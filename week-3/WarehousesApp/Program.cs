using Microsoft.Extensions.DependencyInjection;
using WarehousesApp.Application;
using WarehousesApp.Application.Implementations;
using WarehousesApp.Application.Repositories;
using WarehousesApp.Application.Repositories.Implementations;
using WarehousesApp.Services;
using WarehousesApp.Services.Implementations;

var serviceProvider = new ServiceCollection()
    .AddScoped<IWarehousesManager, WarehousesManager>()
    .AddScoped<IWarehouseService, WarehouseService>()
    .AddScoped<IWarehouseRepository, WarehouseRepositoryCsv>()
    .BuildServiceProvider();

var warehousesManager = serviceProvider.GetRequiredService<IWarehousesManager>();
warehousesManager.StartWarehousesApp();