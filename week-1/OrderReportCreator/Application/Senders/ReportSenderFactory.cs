using OrderReportCreator.Exceptions;

namespace OrderReportCreator.Application.Senders;
public class ReportSenderFactory : IReportSenderFactory
{
    private readonly IEnumerable<IReportSender> _reportSenders;
    public ReportSenderFactory(IEnumerable<IReportSender> reportSenders)
    {
        _reportSenders = reportSenders;
    }
    public IReportSender GetReportSender(ResponseFormat responseFormat)
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