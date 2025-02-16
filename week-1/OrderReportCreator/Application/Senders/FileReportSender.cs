namespace OrderReportCreator.Application.Senders;

public class FileReportSender: IReportSender
{
    public bool CanSend(ResponseFormat responseFormat) 
        => responseFormat == ResponseFormat.File;

    public void Send()
    {
        throw new NotImplementedException();
    }
}