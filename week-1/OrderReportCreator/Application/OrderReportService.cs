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
    public IEnumerable<Report> CreateOrderReport(Request request)
    {
        foreach (var clientId in request.Ids.ToList())
        {
            if (!_orderRepository.TryGetByClientId(clientId))
            {
                throw new GetClientIdException(clientId);
            }
            
            
        }
        
        return new List<Report>();
    }
}