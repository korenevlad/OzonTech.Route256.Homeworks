namespace OrderReportCreator.Application.Senders;

public class ConsoleReportSender: IReportSender
{
    public bool CanSend(ResponseFormat responseFormat) 
        => responseFormat == ResponseFormat.Console;

    public void Send()
    {
        throw new NotImplementedException();
    }
}