using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaHomework.OrderEventConsumer.Domain.Contracts;
using KafkaHomework.OrderEventConsumer.Infrastructure.Kafka;
using KafkaHomework.OrderEventConsumer.Presentation.BLL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaHomework.OrderEventConsumer.Presentation.Kafka;

public class KafkaBackgroundService : BackgroundService
{
    private readonly KafkaAsyncConsumer<long, OrderEvent> _consumer;
    private readonly ILogger<KafkaBackgroundService> _logger;

    public KafkaBackgroundService(IServiceProvider serviceProvider, ILogger<KafkaBackgroundService> logger)
    {
        // TODO: IOptions
        // TODO: KafkaServiceExtensions: services.AddKafkaHandler<TKey, TValue, THandler<TKey, TValue>>(serializers, topic, groupId);
        _logger = logger;
        var handler = serviceProvider.GetRequiredService<KafkaHandler>();
        _consumer = new KafkaAsyncConsumer<long, OrderEvent>(
            handler,
            "kafka:9092",
            "group_id",
            "order_events",
            new SystemTextJsonSerializer<long>(new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            }),
            new SystemTextJsonSerializer<OrderEvent>(new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            }),
            serviceProvider.GetRequiredService<ILogger<KafkaAsyncConsumer<long, OrderEvent>>>());
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Dispose();

        return Task.CompletedTask;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _consumer.Consume(stoppingToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occured");
        }
    }
}
