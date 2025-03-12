using Ozon.Route256.Week4.CustomerService.Extensions;
using Ozon.Route256.Week4.CustomerService.Presentation.Controllers.Grpc;

namespace Ozon.Route256.Week4.CustomerService;

public class Startup
{
    private readonly IConfiguration _configuration;

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
}