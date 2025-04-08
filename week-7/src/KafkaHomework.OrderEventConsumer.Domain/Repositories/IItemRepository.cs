using System;
using System.Threading;
using System.Threading.Tasks;
using KafkaHomework.OrderEventConsumer.Domain.Contracts;

namespace KafkaHomework.OrderEventConsumer.Domain.Repositories;
public interface IItemRepository
{
    Task UpdateItemStatisticsAsync(long itemId, int quantity, OrderEvent.OrderStatus status, DateTime last_updated, CancellationToken token);
}
