using WarehousesApp.Application;

namespace WarehousesApp.Services.Implementations;
public class WarehousesManager : IWarehousesManager
{
    private readonly IWarehouseService _warehouseService;
    public WarehousesManager(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    public async Task StartWarehousesApp(CancellationToken cancellationToken)
    { 
        var (itemsCount, totalCost) = await _warehouseService.GetTotalCost(cancellationToken); 
        Console.WriteLine($"Items count: {itemsCount}, Total cost: {totalCost}");
    }
}