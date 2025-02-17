using OrderReportCreator.Requests;

namespace OrderReportCreator.Presentation;

public interface IUI
{
    GenerateOrderReportRequest GetRequest();
    void SendMessage(string message);
}