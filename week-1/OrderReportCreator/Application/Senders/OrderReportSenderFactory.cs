using OrderReportCreator.Exceptions;

namespace OrderReportCreator.Application.Senders;
public class OrderReportSenderFactory : IOrderReportSenderFactory
{
    private readonly IEnumerable<IOrderReportSender> _reportSenders;
    public OrderReportSenderFactory(IEnumerable<IOrderReportSender> reportSenders)
    {
        _reportSenders = reportSenders;
    }
    public IOrderReportSender GetReportSender(ResponseFormat responseFormat)
    {
        foreach (var reportSender in _reportSenders)
        {
            if (reportSender.CanSendReport(responseFormat))
            {
                return reportSender;
            }
        }
        throw new GetReportSenderException(responseFormat);
    }
}