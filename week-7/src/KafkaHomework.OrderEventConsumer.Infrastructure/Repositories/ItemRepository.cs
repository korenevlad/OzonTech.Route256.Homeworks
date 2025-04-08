using System;
using System.Threading;
using System.Threading.Tasks;
using KafkaHomework.OrderEventConsumer.Domain;
using Microsoft.Extensions.Logging;
using Dapper;
using KafkaHomework.OrderEventConsumer.Domain.Repositories;
using OrderEvent = KafkaHomework.OrderEventConsumer.Domain.Contracts.OrderEvent;

namespace KafkaHomework.OrderEventConsumer.Infrastructure.Repositories;
public sealed class ItemRepository : PgRepository, IItemRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(string connectionString, ILogger<ItemRepository> logger) : base(connectionString)
    {
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task UpdateItemStatisticsAsync(long itemId, int quantity, Domain.Contracts.OrderEvent.OrderStatus status, DateTime last_updated, CancellationToken token)
    {
        var query = status switch
        {
            Domain.Contracts.OrderEvent.OrderStatus.Created => 
                @"insert into product_items (item_id, reserved_count, sold_count, cancelled_count, last_updated) 
values (@itemId, @quantity, 0, 0, @last_updated) 
on conflict (item_id) DO 
update set reserved_count = product_items.reserved_count + @quantity
         , last_updated = @last_updated;",

            Domain.Contracts.OrderEvent.OrderStatus.Delivered => 
                @"update product_items 
set sold_count = sold_count + @quantity
  , reserved_count = reserved_count - @quantity 
  , last_updated = @last_updated 
where item_id = @itemId;",

            Domain.Contracts.OrderEvent.OrderStatus.Canceled => 
                @"update product_items 
set cancelled_count = cancelled_count + @quantity 
  , reserved_count = reserved_count - @quantity 
  , last_updated = @last_updated 
where item_id = @itemId;",

            _ => throw new ArgumentException($"Unknown status: {status}")
        };

        await using var connection = await GetConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                query,
                new
                {
                    itemId = itemId,
                    quantity = quantity,
                    last_updated = last_updated
                },
                cancellationToken: token
                ));
        
        _logger.LogInformation("Updated statistics for item {ItemId} with status {Status}", itemId, status);
    }
}
