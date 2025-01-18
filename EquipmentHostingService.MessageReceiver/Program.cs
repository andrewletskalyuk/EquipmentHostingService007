using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

public class Program
{
    static string _serviceBusConnectionString;
    static string _queueName;

    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

        _serviceBusConnectionString = configuration["AzureServiceBus:ConnectionString"];
        _queueName = configuration["AzureServiceBus:QueueName"];

        var client = new ServiceBusClient(_serviceBusConnectionString);
        var processor = client.CreateProcessor(_queueName, new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += MessageHandler;
        processor.ProcessErrorAsync += ErrorHandler;

        await processor.StartProcessingAsync();

        Console.WriteLine("Waiting for messages...");
        Console.ReadLine();

        await processor.StopProcessingAsync();
        await processor.DisposeAsync();
    }

    private static Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine($"Received message: {body}");

        return Task.CompletedTask;
    }

    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine($"Error occurred: {args.Exception.Message}");
        return Task.CompletedTask;
    }
}