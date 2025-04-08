using System;
using System.Collections.Generic;
using System.Linq;
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
public class KafkaHandler : IHandler<long, OrderEvent>
{
    private readonly ILogger<KafkaHandler> _logger;
    private readonly Random _random = new();
    private readonly IItemRepository _repository;
    private const int maxDegreeOfParallelism = 5;

    public KafkaHandler(ILogger<KafkaHandler> logger, IItemRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }
    
    public async Task Handle(IReadOnlyCollection<ConsumeResult<long, OrderEvent>> messages, CancellationToken token)
    {
        var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
        var messageTasks = messages.Select(async message =>
        {
            try
            {
                var order = message.Message.Value;
                var positionTasks = order.Positions.Select(async position =>
                {
                    await semaphore.WaitAsync(token);
                    try
                    {
                        await _repository.UpdateItemStatisticsAsync(position.ItemId, position.Quantity, order.Status, token);
                        _logger.LogInformation("Finished update for ItemId: {ItemId}", position.ItemId);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
                await Task.WhenAll(positionTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message {Message}", message.Message);
            }
        });
        await Task.WhenAll(messageTasks);
        _logger.LogInformation($"Handled {messages.Count} messages. {DateTime.UtcNow}");
    }
}
