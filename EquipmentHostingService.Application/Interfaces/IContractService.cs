using EquipmentHostingService.Application.Dtos;

namespace EquipmentHostingService.Application.Interfaces;

public interface IContractService
{
    Task<OperationResult<EquipmentPlacementContractDto>> CreateContract(EquipmentPlacementContractDto contractDto);

    Task<IEnumerable<ContractDetailsDto>> GetAllContracts();
}
