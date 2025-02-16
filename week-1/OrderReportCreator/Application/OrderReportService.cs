using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Commands;
public class OrderReportService: IOrderReportService
{
    private readonly IClientRepository _clientRepository;
    public OrderReportService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public IEnumerable<Report> CreateOrderReports()
    {
        throw new NotImplementedException();
    }
}