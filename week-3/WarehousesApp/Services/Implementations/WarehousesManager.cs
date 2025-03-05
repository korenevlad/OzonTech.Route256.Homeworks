using WarehousesApp.Application;

namespace WarehousesApp.Services.Implementations;
public class WarehousesManager : IWarehousesManager
{
    private readonly IWarehouseService _warehouseService;
    public WarehousesManager(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    public async Task StartWarehousesApp()
    { 
        var (itemsCount, totalCost) = await _warehouseService.GetTotalCost(); 
        Console.WriteLine($"Items count: {itemsCount}, Total cost: {totalCost}");
    }
}