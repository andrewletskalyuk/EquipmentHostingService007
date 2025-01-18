namespace EquipmentHostingService.Domain.Interfaces;

public interface IUnitOfWork
{
    Task CompleteAsync();
}
