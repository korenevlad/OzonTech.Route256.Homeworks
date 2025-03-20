using AutoBogus;
using Ozon.Route256.Week4.CustomerService.Domain.Models;

namespace Ozon.Route256.Week4.CustomerService.UnitTests.Fakers;
public static class CustomerFaker
{
    public static Customer Generate()
    {
        var customer = new AutoFaker<Customer>().Generate();
        return customer;
    }

    public static Customer WithId(this Customer customer, long id)
    {
        return customer with
        {
            Id = id
        };
    }
}