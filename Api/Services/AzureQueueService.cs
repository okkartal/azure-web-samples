using System.Text.Json;
using Azure.Storage.Queues;

namespace Api.Services;
public class AzureQueueService : IQueueService
{
    private readonly IConfiguration _configuration;

    public AzureQueueService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task SendMessageAsync<T>(T item)
    {
        string messageBody = JsonSerializer.Serialize(item);

        QueueClient orderQueue = new QueueClient(
            _configuration[Constants.KeyStorageCnn], _configuration[Constants.KeyQueue]);

        await orderQueue.CreateIfNotExistsAsync();

        await orderQueue.SendMessageAsync(messageBody);
    }
}