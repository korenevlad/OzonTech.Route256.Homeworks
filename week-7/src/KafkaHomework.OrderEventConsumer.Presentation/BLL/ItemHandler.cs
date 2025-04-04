using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaHomework.OrderEventConsumer.Infrastructure.Kafka;
using KafkaHomework.OrderEventConsumer.Presentation.Contracts;
using Microsoft.Extensions.Logging;

namespace KafkaHomework.OrderEventConsumer.Presentation.BLL;
public class ItemHandler : IHandler<long, OrderEvent>
{
    private readonly ILogger<ItemHandler> _logger;
    private readonly Random _random = new();

    public ItemHandler(ILogger<ItemHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<long, OrderEvent>> messages, CancellationToken token)
    {
        foreach (var message in messages)
        {
            try
            {
                var order = message.Message.Value;
                _logger.LogInformation(
                    "Received Order: {OrderId}, User: {UserId}, Warehouse: {WarehouseId}, Status: {Status}, Items: {Items}",
                    order.OrderId,
                    order.UserId,
                    order.WarehouseId,
                    order.Status,
                    JsonSerializer.Serialize(order.Positions, new JsonSerializerOptions { WriteIndented = true })
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing message");
            }
        }
        await Task.Delay(_random.Next(1), token);
        _logger.LogInformation($"Handled {messages.Count} messages. {DateTime.UtcNow}");
    }
}
