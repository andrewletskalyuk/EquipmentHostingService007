using EquipmentHostingService.Domain.Entities;
using EquipmentHostingService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EquipmentHostingService.Infrastructure.Data;

public class EquipmentPlacementContractRepository : Repository<EquipmentPlacementContract>, IEquipmentPlacementContractRepository
{
    public EquipmentPlacementContractRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EquipmentPlacementContract>> GetAllWithDetailsAsync()
    {
        return await _entities
            .AsNoTracking()
            .Include(e => e.ProductionFacility)
            .Include(e => e.ProcessEquipmentType)
            .ToListAsync();
    }
}
