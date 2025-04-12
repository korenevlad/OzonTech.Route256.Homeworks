using System;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaHomework.OrderEventConsumer.Domain.Repositories;
public interface IProductSalesRepository
{
    Task UpdateProductSalesAsync(long itemId, long priceUnits, int priceNanos
        , string currency, int quantity, DateTime timestamp, CancellationToken token);
}