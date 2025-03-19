using WarehousesApp.Application.Repositories;

namespace WarehousesApp.Application.Implementations;
public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<(int, double)> GetTotalCost(CancellationToken cancellationToken)
    {
        return await _warehouseRepository.GetTotalCost(cancellationToken);
    }
}