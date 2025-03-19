using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;

namespace Ozon.Route256.Week4.CustomerService.IntegrationTests;

public class CustomCustomerWebHost<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public ICustomerRepository CustomerRepository { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            CustomerRepository = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
        });
    }
}