using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WarehousesApp.Application;
using WarehousesApp.Application.Implementations;
using WarehousesApp.Application.Repositories;
using WarehousesApp.Application.Repositories.Implementations;
using WarehousesApp.Services;
using WarehousesApp.Services.Implementations;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var serviceProvider = new ServiceCollection()
    .AddSingleton<IConfiguration>(config)
    .AddScoped<IWarehousesManager, WarehousesManager>()
    .AddScoped<IWarehouseService, WarehouseService>()
    .AddScoped<IWarehouseRepository, WarehouseRepositoryCsv>()
    .BuildServiceProvider();

var warehousesManager = serviceProvider.GetRequiredService<IWarehousesManager>();
using var tokenSource = new CancellationTokenSource();

// Тестирование CancelationTokenSource
// Console.CancelKeyPress += (sender, eventArgs) =>
// {
//     Console.WriteLine("Запрос отменён!");
//     tokenSource.Cancel();
//     eventArgs.Cancel = true;
// };

await warehousesManager.StartWarehousesApp(tokenSource.Token);