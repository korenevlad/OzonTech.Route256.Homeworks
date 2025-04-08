using KafkaHomework.OrderEventConsumer.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KafkaHomework.OrderEventConsumer.Infrastructure;
using KafkaHomework.OrderEventConsumer.Infrastructure.Common;
using KafkaHomework.OrderEventConsumer.Infrastructure.Repositories;
using KafkaHomework.OrderEventConsumer.Presentation.BLL;
using KafkaHomework.OrderEventConsumer.Presentation.Kafka;
using Microsoft.Extensions.Logging;

namespace KafkaHomework.OrderEventConsumer.Presentation;

public sealed class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddLogging();

        var connectionString = _configuration["ConnectionString"]!;
        services
            .AddFluentMigrator(
                connectionString,
                typeof(SqlMigration).Assembly);
        
        services.AddScoped<IItemRepository, ItemRepository>(serviceProvider => 
            {
                var logger = serviceProvider.GetRequiredService<ILogger<ItemRepository>>();
                return new ItemRepository(connectionString, logger);
            } 
        );
        services.AddSingleton<KafkaHandler>();
        services.AddHostedService<KafkaBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    }
}
