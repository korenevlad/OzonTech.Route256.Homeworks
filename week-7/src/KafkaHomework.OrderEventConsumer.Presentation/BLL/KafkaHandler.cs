using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaHomework.OrderEventConsumer.Domain;
using KafkaHomework.OrderEventConsumer.Domain.Contracts;
using KafkaHomework.OrderEventConsumer.Domain.Repositories;
using KafkaHomework.OrderEventConsumer.Infrastructure;
using KafkaHomework.OrderEventConsumer.Infrastructure.Kafka;
using Microsoft.Extensions.Logging;

namespace KafkaHomework.OrderEventConsumer.Presentation.BLL;
public class KafkaHandler : IHandler<long, OrderEvent>
{
    private readonly IItemRepository _itemRepository;
    private readonly IProductSalesRepository _productSalesRepository;
    private readonly ILogger<KafkaHandler> _logger;
    private const int maxDegreeOfParallelism = 5;

    public KafkaHandler(IItemRepository itemRepository, IProductSalesRepository productSalesRepository, ILogger<KafkaHandler> logger)
    {
        _itemRepository = itemRepository;
        _productSalesRepository = productSalesRepository;
        _logger = logger;
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
                        await _itemRepository.UpdateItemStatisticsAsync(position.ItemId, position.Quantity, order.Status, order.Moment, token);
                        _logger.LogInformation("Finished update for ItemId: {ItemId}", position.ItemId);

                        if (order.Status is OrderEvent.OrderStatus.Delivered)
                        {
                            await _productSalesRepository.UpdateProductSalesAsync(
                                position.ItemId, position.Price.Units, position.Price.Nanos
                                , position.Price.Currency, position.Quantity, order.Moment, token);
                            _logger.LogInformation("Finished update ProductSales for: {ItemId}", position.ItemId);
                        }
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
