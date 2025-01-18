using Azure.Messaging.ServiceBus;
using EquipmentHostingService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EquipmentHostingService.Application.Messaging;

public class ServiceBusSenderService : IServiceBusSender
{
    readonly ServiceBusClient _client;
    readonly string _queueName;
    readonly ILogger<ServiceBusSenderService> _logger;


    public ServiceBusSenderService(IConfiguration configuration, ILogger<ServiceBusSenderService> logger)
    {
        var connectionString = configuration["AzureServiceBus:ConnectionString"];
        _queueName = configuration["AzureServiceBus:QueueName"]!;
        _client = new ServiceBusClient(connectionString);
        _logger = logger;
    }

    public async Task SendMessageAsync(object message)
    {
        var sender = _client.CreateSender(_queueName);
        var jsonMessage = JsonSerializer.Serialize(message);
        var serviceBusMessage = new ServiceBusMessage(jsonMessage);

        await sender.SendMessageAsync(serviceBusMessage);
        _logger.LogInformation("Message sent to Service Bus Queue: {QueueName}, Message: {Message}", _queueName, jsonMessage);

        await sender.DisposeAsync();
    }
}
