namespace OrderReportCreator.Application.Senders;
public class ReportSenderFactory : IReportSenderFactory
{
    private readonly IReportSender[] _reportSenders;

    public IReportSender GetReportSender(ResponseFormat responseFormat)
    {
        foreach (var reportSender in _reportSenders)
        {
            if (reportSender.CanSend(responseFormat))
            {
                return reportSender;
            }
        }
        //TODO: Свой эксепшн?
        throw new ArgumentOutOfRangeException();
    }
}