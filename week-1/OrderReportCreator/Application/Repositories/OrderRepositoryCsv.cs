using System.Globalization;
using CsvHelper;
using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Repositories;
public class OrderRepositoryCsv: IOrderRepository
{
    private const string csvFilePath = @"..\..\..\..\orders_log.csv";
    public bool TryGetByClientId(long id)
    {
        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<OrderMapCsv>();
            var records = csv.GetRecords<Order>().ToList();
            var foundRecord = records.FirstOrDefault(r => r.Client.Id == id);
            var result = foundRecord != null ? true : false;
            return result;
        }
    }
}