namespace Api.Services;

public interface IQueueService
{
    Task SendMessageAsync<T>(T item);
}