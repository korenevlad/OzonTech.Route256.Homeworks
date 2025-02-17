using OrderReportCreator.Domain.Models;
using OrderReportCreator.Domain.Models.OrderAggregate;
using OrderReportCreator.Exceptions;

namespace OrderReportCreator.Application;
public class OrderReportService: IOrderReportService
{
    private readonly IOrderRepository _orderRepository;
    public OrderReportService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public Report CreateOrderReport(IEnumerable<long> ids)
    {
        var report = new Report();
        foreach (var clientId in ids.ToList())
        {
            if (!_orderRepository.TryFindByClientId(clientId))
            {
                throw new FindClientIdException(clientId);
            }
            var favoriteItemName = _orderRepository.GetFavoriteItemNameByClientId(clientId);
            var orderSum = _orderRepository.GetOrderSumByClientId(clientId);
            report.Rows.Add(new ReportRow
            {
                ClientId = clientId,
                OrderSum = orderSum,
                FavoriteItemName = favoriteItemName
            });
        }
        return report ;
    }
}