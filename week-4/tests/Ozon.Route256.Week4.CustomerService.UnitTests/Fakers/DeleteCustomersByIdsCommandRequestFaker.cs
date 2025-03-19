using Ozon.Route256.Week4.CustomerService.DAL.Contracts;
using Ozon.Route256.Week4.CustomerService.Domain.Services.DeleteCustomersByIds;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
public static class DeleteCustomersByIdsCommandRequestFaker
{
    public static DeleteCustomersByIdsCommandRequest GenerateFromCustomers(List<CustomerDbRecord> customers)
    {
        var customerIds = customers.Select(c => c.Id).Distinct().ToArray() ;
        return new DeleteCustomersByIdsCommandRequest(customerIds);
    }
}