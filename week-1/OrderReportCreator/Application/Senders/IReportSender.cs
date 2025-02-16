namespace OrderReportCreator.Application.Senders;

public interface IReportSender
{
    bool CanSend(ResponseFormat responseFormat);
    void Send();
}