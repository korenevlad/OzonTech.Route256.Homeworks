namespace WarehousesApp.Services;

public interface IWarehousesManager
{
    Task StartWarehousesApp(CancellationToken cancellationToken);
}