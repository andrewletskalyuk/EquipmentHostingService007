namespace EquipmentHostingService.Application.Interfaces;

public interface IServiceBusSender
{
    Task SendMessageAsync(object message);
}
