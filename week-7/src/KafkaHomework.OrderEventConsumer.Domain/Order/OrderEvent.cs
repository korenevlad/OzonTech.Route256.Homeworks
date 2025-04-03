using System;
using KafkaHomework.OrderEventConsumer.Domain.ValueObjects;

namespace KafkaHomework.OrderEventConsumer.Domain.Order;

public sealed record OrderEvent(
    OrderId OrderId,
    UserId UserId,
    WarehouseId WarehouseId,
    Status Status,
    DateTime Moment,
    OrderEventPosition[] Positions);
