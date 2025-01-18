using EquipmentHostingService.Domain.Entities;

namespace EquipmentHostingService.Domain.Interfaces;

public interface IEquipmentPlacementContractRepository : IRepository<EquipmentPlacementContract>
{
    Task<IEnumerable<EquipmentPlacementContract>> GetAllWithDetailsAsync();
}
