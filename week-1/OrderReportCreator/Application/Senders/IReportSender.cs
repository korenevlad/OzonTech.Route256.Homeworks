using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Senders;

public interface IReportSender
{
    bool CanSendReport(ResponseFormat responseFormat);
    string SendReport(Report report);
}