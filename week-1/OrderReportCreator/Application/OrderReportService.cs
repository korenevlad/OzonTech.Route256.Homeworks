using OrderReportCreator.Domain.Models;
using OrderReportCreator.Exceptions;
using OrderReportCreator.Requests;

namespace OrderReportCreator.Application;
public class OrderReportService: IOrderReportService
{
    private readonly IOrderRepository _orderRepository;
    public OrderReportService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public IEnumerable<ReportItem> CreateOrderReport(Request request)
    {
        var report = new List<ReportItem>();
        foreach (var clientId in request.Ids.ToList())
        {
            if (!_orderRepository.TryGetByClientId(clientId))
            {
                throw new GetClientIdException(clientId);
            }
            var favoriteItemName = _orderRepository.GetFavoriteItemNameByClientId(clientId);
            var orderSum = _orderRepository.GetOrderSumByClientId(clientId);
            report.Add(new ReportItem
            {
                ClientId = clientId,
                OrderSum = orderSum,
                FavoriteItemName = favoriteItemName
            });
        }
        return report ;
    }
}