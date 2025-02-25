using OrderReportCreator.Domain.Models.ClientAggregate;

namespace OrderReportCreator.Domain.Models;

public class Order
{
    public long Id { get; set; }
    public Client Client { get; set; }
    public Item Item { get; set; }
    public int ItemQuantity { get; set; }
}