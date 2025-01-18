using EquipmentHostingService.Application.Dtos;
using EquipmentHostingService.Application.Interfaces;
using EquipmentHostingService.Domain.Entities;
using EquipmentHostingService.Domain.Interfaces;

namespace EquipmentHostingService.Application.Services;

public class ContractService : IContractService
{
    readonly IEquipmentPlacementContractRepository _contractRepository;
    readonly IRepository<ProductionFacility> _facilityRepository;
    readonly IRepository<ProcessEquipmentType> _equipmentTypeRepository;
    readonly IUnitOfWork _unitOfWork;
    readonly IServiceBusSender _serviceBusSender;

    public ContractService(
        IEquipmentPlacementContractRepository contractRepository,
        IRepository<ProductionFacility> facilityRepository,
        IRepository<ProcessEquipmentType> equipmentTypeRepository,
        IUnitOfWork unitOfWork,
        IServiceBusSender serviceBusSender)
    {
        _contractRepository = contractRepository;
        _facilityRepository = facilityRepository;
        _equipmentTypeRepository = equipmentTypeRepository;
        _unitOfWork = unitOfWork;
        _serviceBusSender = serviceBusSender;
    }

    public async Task<OperationResult<EquipmentPlacementContractDto>> CreateContract(EquipmentPlacementContractDto contractDto)
    {
        var facility = await _facilityRepository.GetByCodeAsync(contractDto.ProductionFacilityCode);
        var equipmentType = await _equipmentTypeRepository.GetByCodeAsync(contractDto.ProcessEquipmentTypeCode);

        if (facility == null || equipmentType == null)
        {
            return new OperationResult<EquipmentPlacementContractDto>(false, "Facility or Equipment Type not found.", null);
        }

        var requiredArea = equipmentType.Area * contractDto.NumberOfUnits;
        if (facility.StandardAreaForEquipment < requiredArea)
        {
            return new OperationResult<EquipmentPlacementContractDto>(false, "Not enough area available in the production facility.", null);
        }

        var contract = new EquipmentPlacementContract
        {
            ProductionFacilityCode = contractDto.ProductionFacilityCode,
            ProcessEquipmentTypeCode = contractDto.ProcessEquipmentTypeCode,
            NumberOfUnits = contractDto.NumberOfUnits,
            ProductionFacility = facility,
            ProcessEquipmentType = equipmentType
        };

        await _contractRepository.AddAsync(contract);
        await _unitOfWork.CompleteAsync();

        // MB
        await _serviceBusSender.SendMessageAsync(new
        {
            ContractId = contract.Id,
            FacilityCode = contract.ProductionFacilityCode,
            EquipmentTypeCode = contract.ProcessEquipmentTypeCode,
            Units = contract.NumberOfUnits
        });

        return new OperationResult<EquipmentPlacementContractDto>(true, "Contract created successfully.", contractDto);
    }

    public async Task<IEnumerable<ContractDetailsDto>> GetAllContracts()
    {
        var contracts = await _contractRepository.GetAllWithDetailsAsync();

        return contracts.Select(c => new ContractDetailsDto
        {
            ProductionFacilityName = c.ProductionFacility.Name,
            ProcessEquipmentTypeName = c.ProcessEquipmentType.Name,
            NumberOfUnits = c.NumberOfUnits
        });
    }
}
