using CsvHelper.Configuration;
using OrderReportCreator.Domain.Models;
using OrderReportCreator.Domain.Models.ClientAggregate;

namespace OrderReportCreator.Application.Repositories;
public class OrderMapCsv : ClassMap<Order>
{
    public OrderMapCsv()
    {
        Map(o => o.Id).Name("order_id");
        Map(o => o.ItemQuantity).Name("item_quantity");
        Map(o => o.Client).Convert(row => new Client 
        { 
            Id = row.Row.GetField<long>("client_id") 
        });
        Map(o => o.Item).Convert(row => new Item
        {
            Name = row.Row.GetField<string>("item_name"),
            Price = row.Row.GetField<float>("item_price")
        });
    }
}
