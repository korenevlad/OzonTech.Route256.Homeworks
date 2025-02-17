using System.Globalization;
using CsvHelper;
using OrderReportCreator.Domain.Models;
using OrderReportCreator.Domain.Models.OrderAggregate;

namespace OrderReportCreator.Application.Repositories;
public class OrderRepositoryCsv: IOrderRepository
{
    private const string CsvFilePath = @"..\..\..\..\orders_log.csv";
    public bool TryFindByClientId(long id)
    {
        var orders = GetOrders();
        var isExistClient = orders.FirstOrDefault(r => r.Client.Id == id);
        return isExistClient != null ? true : false;
    }

    public string GetFavoriteItemNameByClientId(long id)
    {
        var orders = GetOrders();
        var clientOrders = orders.Where(o => o.Client.Id == id).ToList();
        var itemFrequency = clientOrders
            .GroupBy(o => o.Item.Name)
            .Select(g => new
            {
                ItemName = g.Key,
                TotalQuantity = g.Sum(o => o.ItemQuantity)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .FirstOrDefault();
        return itemFrequency.ItemName;
    }

    public float GetOrderSumByClientId(long id)
    {
        var orders = GetOrders();
        return orders
            .Where(o => o.Client.Id == id)
            .Sum(o => o.ItemQuantity * o.Item.Price);
    }

    private List<Order> GetOrders()
    {
        using (var reader = new StreamReader(CsvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<OrderMapCsv>();
            return csv.GetRecords<Order>().ToList();
        }
    }
}