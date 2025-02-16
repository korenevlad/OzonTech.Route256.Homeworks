namespace OrderReportCreator.Application.Senders;

public interface IReportSenderFactory
{
    IReportSender GetReportSender(ResponseFormat responseFormat);
}