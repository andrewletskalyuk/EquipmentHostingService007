using EquipmentHostingService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EquipmentHostingService.Infrastructure.Data;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;

    protected readonly DbSet<T> _entities;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _entities = context.Set<T>();
    }

    public async Task<T> GetByCodeAsync(string code) => await _entities.FirstOrDefaultAsync(e => EF.Property<string>(e, "Code") == code);

    public async Task AddAsync(T entity) => await _entities.AddAsync(entity);

    public void Update(T entity) => _entities.Update(entity);

    public void Remove(T entity) => _entities.Remove(entity);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
