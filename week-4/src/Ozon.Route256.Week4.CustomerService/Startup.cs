using Ozon.Route256.Week4.CustomerService.DAL;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.Extensions;
using Ozon.Route256.Week4.CustomerService.Presentation.Controllers.Grpc;

namespace Ozon.Route256.Week4.CustomerService;

public class Startup
{
    private IConfiguration _configuration { get; }

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var currentAssembly = typeof(Startup).Assembly;
        services
            .AddMediatR(c => c.RegisterServicesFromAssembly(currentAssembly));

        services
            .AddRepositories();

        services.AddControllers();
        services.AddGrpc(
                options =>
                {
                    options.EnableDetailedErrors = true;
                })
            .AddJsonTranscoding();
        services.AddGrpcSwagger();
        services.AddSwaggerGen();
        services.AddGrpcReflection();

        AddServices(services, _configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<CustomerController>();
                endpoints.MapGrpcReflectionService();
            });
    }
    
    private static void AddServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository>();
    }

}