using OrderReportCreator.Application;
using OrderReportCreator.Application.Senders;
using OrderReportCreator.Presentation;

namespace OrderReportCreator.Services;
public class OrderReportManager : IOrderReportManager
{
    private readonly IUI _ui;
    private readonly IOrderReportService _orderReportService;
    private readonly IReportSenderFactory _reportSenderFactory;
    
    public OrderReportManager(IUI ui, IOrderReportService orderReportService)
    {
        _ui = ui;
        _orderReportService = orderReportService;
    }
    public void GenerateOrderReport()
    {
        // забрать запрос 
        var request = _ui.GetRequest();
        
        // собрать отчёт
        _orderReportService.CreateOrderReport(request);
        
        // выбрать вариант, как мы его отдадим
        var reportSender = _reportSenderFactory.GetReportSender(request.ResponseFormat);
        
        // отдать его юзеру
        reportSender.Send();
    }
}