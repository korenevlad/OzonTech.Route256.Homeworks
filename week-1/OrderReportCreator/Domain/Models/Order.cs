namespace OrderReportCreator.Domain.Models;

public class Order
{
    public Client Client { get; set; }
    public Item Item { get; set; }
    public int ItemQuantity { get; set; }
}