using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ContactService.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqEventPublisher> _logger;

    public RabbitMqEventPublisher(
        IConfiguration configuration,
        ILogger<RabbitMqEventPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string queueName, T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:HostName"],
            UserName = _configuration["RabbitMq:UserName"],
            Password = _configuration["RabbitMq:Password"],
            AutomaticRecoveryEnabled = true
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        _logger.LogInformation(
            "Published event {EventType} to queue {QueueName}",
            typeof(T).Name,
            queueName);
    }
}