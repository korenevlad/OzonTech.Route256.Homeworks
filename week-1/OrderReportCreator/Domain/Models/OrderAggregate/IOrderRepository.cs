namespace OrderReportCreator.Domain.Models;

public interface IOrderRepository
{
    bool TryGetByClientId(long id);
}