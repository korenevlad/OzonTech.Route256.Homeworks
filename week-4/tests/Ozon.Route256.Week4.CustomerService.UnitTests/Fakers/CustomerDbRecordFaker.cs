using AutoBogus;
using Ozon.Route256.Week4.CustomerService.DAL.Contracts;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
public class CustomerDbRecordFaker
{
    public static List<CustomerDbRecord> GenerateListForGetCustomersQuery(
        int count = 5, 
        bool includeCustomerIds = true, 
        bool includeRegionIds = true, 
        bool includeFullNames = true)
    {
        var faker = new AutoFaker<CustomerDbRecord>();

        if (includeCustomerIds)
        {
            faker.RuleFor(c => c.Id, f => f.Random.Long(1, 1000));
        }
        else
        {
            faker.RuleFor(c => c.Id, _ => 0);
        }

        if (includeRegionIds)
        {
            faker.RuleFor(c => c.RegionId, f => f.Random.Long(1, 1000));
        }
        else
        {
            faker.RuleFor(c => c.RegionId, _ => 0);
        }

        if (includeFullNames)
        {
            faker.RuleFor(c => c.FullName, f => f.Name.FullName());
        }
        else
        {
            faker.RuleFor(c => c.FullName, _ => string.Empty);
        }

        return faker.Generate(count);
    }
    
    public static List<CustomerDbRecord> GenerateListForDeleteCustomersByIds(
        int count = 5)
    {
        var faker = new AutoFaker<CustomerDbRecord>();
        faker.RuleFor(c => c.Id, f => f.Random.Long(1, 1000));
        return faker.Generate(count);
    }
}