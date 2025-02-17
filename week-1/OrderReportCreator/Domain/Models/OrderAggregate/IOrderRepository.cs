namespace OrderReportCreator.Domain.Models.OrderAggregate;

public interface IOrderRepository
{
    bool TryFindByClientId(long id);
    string GetFavoriteItemNameByClientId(long id);
    float GetOrderSumByClientId(long id);
}