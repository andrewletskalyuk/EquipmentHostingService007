namespace EquipmentHostingService.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetByCodeAsync(string code);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task SaveChangesAsync();
}
