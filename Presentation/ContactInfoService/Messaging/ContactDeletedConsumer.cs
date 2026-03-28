using System.Text;
using System.Text.Json;
using PhoneBook.Contracts.Events;
using PhoneBook.Services.ContactInfos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ContactInfoService.Messaging;

public class ContactDeletedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public ContactDeletedConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:HostName"],
            UserName = _configuration["RabbitMq:UserName"],
            Password = _configuration["RabbitMq:Password"],
            AutomaticRecoveryEnabled = true
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: "contact-deleted-queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null)
            return;

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var message = JsonSerializer.Deserialize<ContactDeletedEvent>(json);

            if (message is not null)
            {
                using var scope = _scopeFactory.CreateScope();
                var contactInfoService = scope.ServiceProvider.GetRequiredService<IContactInfoService>();

                await contactInfoService.SoftDeleteByContactIdAsync(message.ContactId);
            }

            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
        };

        await _channel.BasicConsumeAsync(
            queue: "contact-deleted-queue",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.CloseAsync(cancellationToken);

        if (_connection is not null)
            await _connection.CloseAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
    }
}