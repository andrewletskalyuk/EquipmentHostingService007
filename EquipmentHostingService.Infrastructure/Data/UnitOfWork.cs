using EquipmentHostingService.Domain.Interfaces;

namespace EquipmentHostingService.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }
}
