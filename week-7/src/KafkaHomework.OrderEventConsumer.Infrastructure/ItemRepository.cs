using KafkaHomework.OrderEventConsumer.Domain;

namespace KafkaHomework.OrderEventConsumer.Infrastructure;

public sealed class ItemRepository : IItemRepository
{
    private readonly string _connectionString;

    public ItemRepository(string connectionString) => _connectionString = connectionString;
}
