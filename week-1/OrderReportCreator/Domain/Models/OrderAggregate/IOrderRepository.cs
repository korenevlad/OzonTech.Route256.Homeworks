namespace OrderReportCreator.Domain.Models;

public interface IOrderRepository
{
    bool TryGetByClientId(long id);
    string GetFavoriteItemNameByClientId(long id);
    float GetOrderSumByClientId(long id);
}