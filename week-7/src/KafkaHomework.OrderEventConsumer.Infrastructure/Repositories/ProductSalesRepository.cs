using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using KafkaHomework.OrderEventConsumer.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace KafkaHomework.OrderEventConsumer.Infrastructure.Repositories;
public class ProductSalesRepository : PgRepository, IProductSalesRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ProductSalesRepository> _logger;
    public ProductSalesRepository(string connectionString, ILogger<ProductSalesRepository> logger) : base(connectionString)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task UpdateProductSalesAsync(long itemId, long priceUnits, int priceNanos
        , string currency, int quantity, DateTime timestamp, CancellationToken token)
    {
        var sellerId = ExtractSellerId(itemId);
        var amount = (priceUnits + priceNanos / 1_000_000_000m) * quantity;
        
        const string sql = @"
insert into product_sales (seller_id, currency, total_sales, total_quantity, last_updated)
values (@SellerId, @Currency, @Amount, @Quantity, @Timestamp)
on conflict (seller_id, currency) do update set
    total_sales = product_sales.total_sales + excluded.total_sales
  , total_quantity = product_sales.total_quantity + excluded.total_quantity
  , last_updated = excluded.last_updated;";

        
        await using var connection = await GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    SellerId = sellerId,
                    Currency = currency,
                    Amount = amount,
                    Quantity = quantity,
                    Timestamp = timestamp
                },
                cancellationToken: token));
    }
    private static long ExtractSellerId(long itemId)
    {
        return itemId / 100_000 * 100_000;
    }
}