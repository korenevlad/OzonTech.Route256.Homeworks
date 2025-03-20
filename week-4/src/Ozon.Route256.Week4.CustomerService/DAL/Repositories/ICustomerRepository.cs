using Ozon.Route256.Week4.CustomerService.DAL.Contracts;

namespace Ozon.Route256.Week4.CustomerService.DAL.Repositories;

public interface ICustomerRepository
{
    Task<long> CreateCustomer(string fullName, long regionId, CancellationToken token);
    Task<CustomerDbRecord[]> GetCustomers(long[] customerIds, long[] regionIds, string[] fullNames, CancellationToken token);
    Task DeleteCustomers(long[] customersIds, CancellationToken token);
}