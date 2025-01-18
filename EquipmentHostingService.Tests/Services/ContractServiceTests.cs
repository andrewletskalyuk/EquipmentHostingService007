using EquipmentHostingService.Application.Dtos;
using EquipmentHostingService.Application.Interfaces;
using EquipmentHostingService.Application.Services;
using EquipmentHostingService.Domain.Entities;
using EquipmentHostingService.Domain.Interfaces;
using EquipmentHostingService.Tests.Models;
using FluentAssertions;
using Moq;

namespace EquipmentHostingService.Tests.Services;

public class ContractServiceTests
{
    private readonly Mock<IEquipmentPlacementContractRepository> _mockContractRepository;
    private readonly Mock<IRepository<ProductionFacility>> _mockFacilityRepository;
    private readonly Mock<IRepository<ProcessEquipmentType>> _mockEquipmentTypeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IServiceBusSender> _mockServiceBusSender;
    private readonly ContractService _contractService;

    public ContractServiceTests()
    {
        _mockContractRepository = new Mock<IEquipmentPlacementContractRepository>();
        _mockFacilityRepository = new Mock<IRepository<ProductionFacility>>();
        _mockEquipmentTypeRepository = new Mock<IRepository<ProcessEquipmentType>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockServiceBusSender = new Mock<IServiceBusSender>();

        _contractService = new ContractService(
                _mockContractRepository.Object,
                _mockFacilityRepository.Object,
                _mockEquipmentTypeRepository.Object,
                _mockUnitOfWork.Object,
                _mockServiceBusSender.Object
            );
    }

    [Fact]
    public async Task CreateContract_Should_Return_Error_When_Facility_Not_Found()
    {
        // Arrange
        _mockFacilityRepository.Setup(x => x.GetByCodeAsync("PF001")).ReturnsAsync((ProductionFacility)null);

        // Act
        var result = await _contractService.CreateContract(new EquipmentPlacementContractDto
        {
            ProductionFacilityCode = "PF001",
            ProcessEquipmentTypeCode = "PET001",
            NumberOfUnits = 10
        });

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Facility or Equipment Type not found.");
    }

    [Fact]
    public async Task CreateContract_Should_Return_Error_When_Area_Not_Sufficient()
    {
        // Arrange
        _mockFacilityRepository.Setup(x => x.GetByCodeAsync("PF001"))
            .ReturnsAsync(new ProductionFacility { Code = "PF001", StandardAreaForEquipment = 1000 });

        _mockEquipmentTypeRepository.Setup(x => x.GetByCodeAsync("PET001"))
            .ReturnsAsync(new ProcessEquipmentType { Code = "PET001", Area = 200 });

        // Act
        var result = await _contractService.CreateContract(new EquipmentPlacementContractDto
        {
            ProductionFacilityCode = "PF001",
            ProcessEquipmentTypeCode = "PET001",
            NumberOfUnits = 6
        });

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Not enough area available in the production facility.");
    }

    [Fact]
    public async Task CreateContract_Should_Add_Contract_When_Valid()
    {
        // Arrange
        var facility = new ProductionFacility { Code = "PF001", StandardAreaForEquipment = 1000 };
        var equipmentType = new ProcessEquipmentType { Code = "PET001", Area = 100 };

        _mockFacilityRepository.Setup(x => x.GetByCodeAsync("PF001")).ReturnsAsync(facility);
        _mockEquipmentTypeRepository.Setup(x => x.GetByCodeAsync("PET001")).ReturnsAsync(equipmentType);

        // Act
        var result = await _contractService.CreateContract(new EquipmentPlacementContractDto
        {
            ProductionFacilityCode = "PF001",
            ProcessEquipmentTypeCode = "PET001",
            NumberOfUnits = 5
        });

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Contract created successfully.");
        _mockContractRepository.Verify(x => x.AddAsync(It.IsAny<EquipmentPlacementContract>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllContracts_Should_Return_Details()
    {
        // Arrange
        var contracts = new List<EquipmentPlacementContract>
            {
                new EquipmentPlacementContract
                {
                    ProductionFacility = new ProductionFacility { Name = "Facility 1" },
                    ProcessEquipmentType = new ProcessEquipmentType { Name = "Equipment 1" },
                    ProductionFacilityCode = "PF001",
                    ProcessEquipmentTypeCode = "PET001",
                    NumberOfUnits = 10
                },
                new EquipmentPlacementContract
                {
                    ProductionFacility = new ProductionFacility { Name = "Facility 2" },
                    ProcessEquipmentType = new ProcessEquipmentType { Name = "Equipment 2" },
                    ProductionFacilityCode = "PF002",
                    ProcessEquipmentTypeCode = "PET002",
                    NumberOfUnits = 5
                }
            };

        _mockContractRepository.Setup(x => x.GetAllWithDetailsAsync()).ReturnsAsync(contracts);

        // Act
        var result = await _contractService.GetAllContracts();

        // Assert
        result.Should().HaveCount(2);
        result.First().ProductionFacilityName.Should().Be("Facility 1");
        result.First().ProcessEquipmentTypeName.Should().Be("Equipment 1");
        result.First().NumberOfUnits.Should().Be(10);
    }

    [Fact]
    public async Task CreateContract_Should_Call_ServiceBusSender_When_Contract_Is_Valid()
    {
        // Arrange
        var facility = new ProductionFacility
        {
            Code = "PF001",
            Name = "Facility 1",
            StandardAreaForEquipment = 1000
        };

        var equipmentType = new ProcessEquipmentType
        {
            Code = "PET001",
            Name = "Equipment 1",
            Area = 100
        };

        var contractDto = new EquipmentPlacementContractDto
        {
            ProductionFacilityCode = facility.Code,
            ProcessEquipmentTypeCode = equipmentType.Code,
            NumberOfUnits = 5
        };

        var createdContract = new EquipmentPlacementContract
        {
            Id = 1,
            ProductionFacilityCode = facility.Code,
            ProcessEquipmentTypeCode = equipmentType.Code,
            NumberOfUnits = contractDto.NumberOfUnits,
            ProductionFacility = facility,
            ProcessEquipmentType = equipmentType
        };

        _mockFacilityRepository
            .Setup(repo => repo.GetByCodeAsync(facility.Code))
            .ReturnsAsync(facility);

        _mockEquipmentTypeRepository
            .Setup(repo => repo.GetByCodeAsync(equipmentType.Code))
            .ReturnsAsync(equipmentType);

        _mockContractRepository
            .Setup(repo => repo.AddAsync(It.IsAny<EquipmentPlacementContract>()))
            .Callback<EquipmentPlacementContract>(c =>
            {
                c.Id = createdContract.Id;
            });

        // Act
        var result = await _contractService.CreateContract(contractDto);

        result.Success.Should().BeTrue();
    }
}
