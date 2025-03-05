namespace WarehousesApp.Application.Repositories;

public interface IWarehouseRepository
{
    Task<(int, double)> GetTotalCost(CancellationToken cancellationToken);
}