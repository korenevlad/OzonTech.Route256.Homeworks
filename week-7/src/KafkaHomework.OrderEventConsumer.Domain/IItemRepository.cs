using System.Threading;
using System.Threading.Tasks;
using KafkaHomework.OrderEventConsumer.Domain.Contracts;

namespace KafkaHomework.OrderEventConsumer.Domain;
public interface IItemRepository
{
    Task UpdateItemStatisticsAsync(long itemId, int quantity, OrderEvent.OrderStatus status, CancellationToken token);
}
