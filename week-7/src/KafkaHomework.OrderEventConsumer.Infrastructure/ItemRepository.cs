using System;
using System.Threading;
using System.Threading.Tasks;
using KafkaHomework.OrderEventConsumer.Domain;
using Microsoft.Extensions.Logging;
using Npgsql;
using OrderEvent = KafkaHomework.OrderEventConsumer.Domain.Contracts.OrderEvent;

namespace KafkaHomework.OrderEventConsumer.Infrastructure;
public sealed class ItemRepository : IItemRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ItemRepository> _logger;

    public ItemRepository(string connectionString, ILogger<ItemRepository> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task UpdateItemStatisticsAsync(long itemId, int quantity, Domain.Contracts.OrderEvent.OrderStatus status, CancellationToken token)
    {
        var query = status switch
        {
            Domain.Contracts.OrderEvent.OrderStatus.Created => 
                "insert into product_items (item_id, reserved_count, sold_count, cancelled_count, last_updated) " + 
                "values (@itemId, @quantity, 0, 0, now()) " + 
                "on conflict (item_id) DO " + 
                "update set reserved_count = product_items.reserved_count + @quantity, last_updated = now();",

            Domain.Contracts.OrderEvent.OrderStatus.Delivered => 
                "update product_items " +
                "set sold_count = sold_count + @quantity, " + 
                "last_updated = now() " +  
                "where item_id = @itemId;", 

            OrderEvent.OrderStatus.Canceled => 
                "update product_items " +
                "set cancelled_count = cancelled_count + @quantity, " +  
                "last_updated = now() " +  
                "where item_id = @itemId;",

            _ => throw new ArgumentException($"Unknown status: {status}")
        };
        
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(token);
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("itemId", itemId);
        command.Parameters.AddWithValue("quantity", quantity);
        
        await command.ExecuteNonQueryAsync(token); 
        _logger.LogInformation("Updated statistics for item {ItemId} with status {Status}", itemId, status);
    }
}
