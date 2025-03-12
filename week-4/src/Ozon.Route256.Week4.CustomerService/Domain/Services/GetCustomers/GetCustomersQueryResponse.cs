using Ozon.Route256.Week4.CustomerService.Domain.Models;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

public sealed class GetCustomersQueryResponse : ResponseBase
{
    public long TotalCount { get; }

    public Customer[] Customers { get; }

    public GetCustomersQueryResponse(Customer[] customers, long totalCount)
    {
        Customers = customers;
        TotalCount = totalCount;
    }
}