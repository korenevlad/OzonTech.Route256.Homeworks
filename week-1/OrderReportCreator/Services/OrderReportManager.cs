using OrderReportCreator.Application;
using OrderReportCreator.Application.Senders;
using OrderReportCreator.Presentation;

namespace OrderReportCreator.Services;
public class OrderReportManager : IOrderReportManager
{
    private readonly IUI _ui;
    private readonly IOrderReportService _orderReportService;
    private readonly IReportSenderFactory _reportSenderFactory;
    public OrderReportManager(
        IUI ui, 
        IOrderReportService orderReportService, 
        IReportSenderFactory reportSenderFactory)
    {
        _ui = ui;
        _orderReportService = orderReportService;
        _reportSenderFactory = reportSenderFactory;
    }
    public void GenerateOrderReport()
    {
        var request = _ui.GetRequest();
        var report = _orderReportService.CreateOrderReport(request.Ids);
        var reportSender = _reportSenderFactory.GetReportSender(request.ResponseFormat);
        var message = reportSender.SendReport(report);
        _ui.SendMessage(message);
    }
}