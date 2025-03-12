using Ozon.Route256.Week4.CustomerService.DAL;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;

namespace Ozon.Route256.Week4.CustomerService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
        return services;
    }
}