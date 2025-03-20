using AutoBogus;
using Ozon.Route256.Week4.CustomerService.DAL.Contracts;
using Ozon.Route256.Week4.CustomerService.Domain.Models;
using Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
public static class GetCustomersQueryRequestFaker
{
    public static GetCustomersQueryRequest GenerateFromCustomers(
        List<CustomerDbRecord> customers,
        bool includeCustomerIds = true,
        bool includeRegionIds = true,
        bool includeFullNames = true)
    {
        var customerIds = includeCustomerIds 
            ? customers.Select(c => c.Id).Distinct().ToArray() 
            : Array.Empty<long>();
        var regionIds = includeRegionIds 
            ? customers.Select(c => c.RegionId).Distinct().ToArray() 
            : Array.Empty<long>();
        var fullNames = includeFullNames
            ? customers.Select(c => c.FullName).Distinct().ToArray()
            : Array.Empty<string>();
        return new GetCustomersQueryRequest(customerIds,regionIds,fullNames);
    }
}