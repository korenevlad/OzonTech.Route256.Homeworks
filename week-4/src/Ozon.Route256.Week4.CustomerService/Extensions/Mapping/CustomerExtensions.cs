using Ozon.Route256.Week4.CustomerService.DAL.Contracts;
using Ozon.Route256.Week4.CustomerService.Domain.Models;

namespace Ozon.Route256.Week4.CustomerService.Extensions.Mapping;

public static class CustomerExtensions
{
    public static Customer[] ToBll(this CustomerDbRecord[] dbRecords)
    {
        return dbRecords.Select(x => new Customer
        {
            Id = x.Id,
            FullName = x.FullName,
            CreatedAt = x.CreatedAt,
        }).ToArray();
    }
}