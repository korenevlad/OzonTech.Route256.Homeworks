using System;

namespace KafkaHomework.OrderEventConsumer.Presentation.Models;
public class ItemStatistics
{
    public long ItemId { get; set; }
    public int ReservedCount { get; set; }
    public int SoldCount { get; set; }
    public int CancelledCount { get; set; }
    public DateTime LastUpdated { get; set; }
}