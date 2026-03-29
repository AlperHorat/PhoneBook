namespace ContactService.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<T>(string queueName, T message);
}