namespace OrderReportCreator.Application.Senders;

public interface IOrderReportSenderFactory
{
    IOrderReportSender GetReportSender(ResponseFormat responseFormat);
}