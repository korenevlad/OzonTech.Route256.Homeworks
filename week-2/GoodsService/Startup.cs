using Calzolari.Grpc.AspNetCore.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using GoodService.DAL.Repositories;
using GoodService.DAL.Repositories.Implementation;
using GoodsService.BLL;
using GoodsService.Presentation.Grpc;
using GoodsService.Presentation.Interceptors;
using GoodsService.Validators;

namespace GoodsService;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddFluentValidationAutoValidation();
        // services.AddValidatorsFromAssemblyContaining(typeof( ));
        
        services.AddSwaggerGen();
        // services.AddMvc(
        //     x =>
        //     {
        //         x.Filters.Add(typeof( ));
        //     });
        services.AddGrpc(
                options =>
                {
                    options.EnableMessageValidation();
                    options.Interceptors.Add<BusinessExceptionInterceptor>();
                })
            .AddJsonTranscoding();

        services.AddGrpcReflection();
        services.AddGrpcSwagger();
        services.AddSwaggerGen();
        services.AddGrpcValidation();
        
        services.AddSingleton<IGoodService, BLL.Implementations.GoodService>();
        services.AddSingleton<IGoodRepository, GoodRepositoryDictionary>();
        services.AddValidator<AddGoodRequestGrpcValidator>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcGoodsService>();
                endpoints.MapGrpcReflectionService();
            });
    }
}
