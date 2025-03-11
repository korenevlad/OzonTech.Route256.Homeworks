using Calzolari.Grpc.AspNetCore.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using GoodService.DAL.Repositories;
using GoodService.DAL.Repositories.Implementation;
using GoodsService.BLL;
using GoodsService.Presentation.Grpc;
using GoodsService.Presentation.Grpc.Interceptors;
using GoodsService.Presentation.Middleware;
using GoodsService.Validators.Grpc;
using GoodsService.Validators.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

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
        
        // Валидация Http
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining(typeof(AddGoodRequestValidator));
        services.AddValidatorsFromAssemblyContaining(typeof(GetGoodsWithFiltersRequestValidator));
        services.AddValidatorsFromAssemblyContaining(typeof(GetGoodByIdRequestValidator));
        
        // Фильтры Http
        services.AddSwaggerGen();
        services.AddMvc(
            x =>
            {
                x.Filters.Add(typeof(BusinessExceptionFilter));
            });
        // Интерцепторы Grpc
        services.AddGrpc(
                options =>
                {
                    options.EnableMessageValidation();
                    options.Interceptors.Add<BusinessExceptionInterceptor>();
                    options.Interceptors.Add<LoggingInterceptor>();
                })
            .AddJsonTranscoding();

        //Grpc
        services.AddGrpcReflection();
        services.AddGrpcSwagger();
        services.AddSwaggerGen(c =>
        {
            c.MapType<GoodsService.Grps.GoodType>(() => new OpenApiSchema
            {
                Enum = Enum.GetValues(typeof(GoodsService.Grps.GoodType))
                    .Cast<int>() 
                    .Where(value => value != (int)GoodsService.Grps.GoodType.Unspecified)
                    .Select(value => new OpenApiInteger(value))
                    .ToList<IOpenApiAny>()
            });
        });
        services.AddGrpcValidation();
        
        //DI
        services.AddSingleton<IGoodService, BLL.Implementations.GoodService>();
        services.AddSingleton<IGoodRepository, GoodRepositoryDictionary>();
        
        //Валидация Grpc
        services.AddValidator<AddGoodRequestGrpcValidator>();
        services.AddValidator<GetGoodByIdRequestGrpcValidator>();
        services.AddValidator<GetGoodsWithFiltersRequestGrpcValidator>();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        // Логирование
        app.UseMiddleware<LoggingMiddleware>();
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
