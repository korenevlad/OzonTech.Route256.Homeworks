namespace WarehousesApp.Application;

public interface IWarehouseService
{
    Task<(int, double)> GetTotalCost(CancellationToken cancellationToken);
}