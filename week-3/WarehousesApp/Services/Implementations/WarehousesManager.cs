using WarehousesApp.Application;

namespace WarehousesApp.Services.Implementations;
public class WarehousesManager : IWarehousesManager
{
    private readonly IWarehouseService _warehouseService;
    public WarehousesManager(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    public void StartWarehousesApp()
    {
        _warehouseService.
    }
}