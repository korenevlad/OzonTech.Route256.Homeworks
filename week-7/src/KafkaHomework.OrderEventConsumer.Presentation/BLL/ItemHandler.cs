using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaHomework.OrderEventConsumer.Domain;
using KafkaHomework.OrderEventConsumer.Domain.Contracts;
using KafkaHomework.OrderEventConsumer.Infrastructure;
using KafkaHomework.OrderEventConsumer.Infrastructure.Kafka;
using Microsoft.Extensions.Logging;

namespace KafkaHomework.OrderEventConsumer.Presentation.BLL;
public class ItemHandler : IHandler<long, OrderEvent>
{
    private readonly ILogger<ItemHandler> _logger;
    private readonly Random _random = new();
    private readonly IItemRepository _repository;

    public ItemHandler(ILogger<ItemHandler> logger, IItemRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<long, OrderEvent>> messages, CancellationToken token)
    {
        foreach (var message in messages)
        {
            try
            {
                var order = message.Message.Value;
                var tasks = new List<Task>();
                foreach (var position in order.Positions)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await _repository.UpdateItemStatisticsAsync(position.ItemId, position.Quantity, order.Status, token);
                        _logger.LogInformation("Finished update for ItemId: {ItemId}", position.ItemId);
                    }));
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message {Message}", message.Message);
            }
        }
        await Task.Delay(_random.Next(300), token);
        
        _logger.LogInformation($"Handled {messages.Count} messages. {DateTime.UtcNow}");
    }
}
