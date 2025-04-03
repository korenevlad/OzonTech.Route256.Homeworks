using KafkaHomework.OrderEventConsumer.Domain.ValueObjects;

namespace KafkaHomework.OrderEventConsumer.Domain.Order;

public sealed record OrderEventPosition(ItemId ItemId, int Quantity, Money Price);
