using System.Collections.Concurrent;
using Ozon.Route256.Week4.CustomerService.DAL.Contracts;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories.Exceptions;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;

namespace Ozon.Route256.Week4.CustomerService.DAL;

public class CustomerRepository : ICustomerRepository
{
    private static long _lastId;
    private readonly ConcurrentDictionary<long, CustomerDbRecord> _storage = new();
    
    public Task<long> CreateCustomer(string fullName, long regionId, CancellationToken token)
    {
        var newId = Interlocked.Increment(ref _lastId);

        var newDbRecord = new CustomerDbRecord
        {
            Id = newId,
            FullName = fullName,
            RegionId = regionId,
            CreatedAt = DateTime.UtcNow
        };
        
        if(!_storage.TryAdd(newId, newDbRecord))
        {
            throw new CustomerAlreadyExistsException($"Customer with id '{newId}' already exists.");
        }

        return Task.FromResult(newId);
    }

    public async Task DeleteCustomers(long[] customersIds, CancellationToken token)
    {
        var customersToDelete = await GetCustomersByFilters(customersIds, Array.Empty<long>(), Array.Empty<string>());

        foreach (var customer in customersToDelete)
        {
            _storage.TryRemove(customer.Id, out _);
        }
    }

    public async Task<CustomerDbRecord[]> GetCustomers(
        long[] customerIds,
        long[] regionIds,
        string[] fullNames,
        CancellationToken token)
    {
        var customers = await GetCustomersByFilters(customerIds, regionIds, fullNames);      

        return customers;
    }
    
    private Task<CustomerDbRecord[]> GetCustomersByFilters(
        long[] customerIds,
        long[] regionIds,
        string[] fullNames)
    {
        var dbRecords = _storage.Values.AsEnumerable();
        
        if (customerIds.Any())
        {
            dbRecords = dbRecords.Where(x => customerIds.Contains(x.Id));
        }
        
        if (fullNames.Any())
        {
            dbRecords = dbRecords.Where(x => fullNames.Contains(x.FullName));
        }
        
        if (regionIds.Any())
        {
            dbRecords = dbRecords.Where(x => regionIds.Contains(x.RegionId));
        }

        return Task.FromResult(dbRecords.ToArray());
    }
}