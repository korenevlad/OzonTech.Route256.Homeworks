namespace KafkaHomework.OrderEventConsumer.Domain.ValueObjects;

public readonly record struct Money(decimal Value, string Currency);
