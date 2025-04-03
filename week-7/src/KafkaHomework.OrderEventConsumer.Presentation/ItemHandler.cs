using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using KafkaHomework.OrderEventConsumer.Infrastructure.Kafka;

namespace KafkaHomework.OrderEventConsumer.Presentation;

public class ItemHandler : IHandler<Ignore, string>
{
    private readonly ILogger<ItemHandler> _logger;
    private readonly Random _random = new();

    public ItemHandler(ILogger<ItemHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, string>> messages, CancellationToken token)
    {
        await Task.Delay(_random.Next(300), token);
        _logger.LogInformation($"Handled {messages.Count} messages. {DateTime.UtcNow}");
    }
}
