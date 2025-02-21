using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Senders;

public interface IOrderReportSender
{
    bool CanSendReport(ResponseFormat responseFormat);
    string SendReport(Report report);
}