using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Senders;
public class ConsoleOrderReportSender: IOrderReportSender
{
    public bool CanSendReport(ResponseFormat responseFormat) 
        => responseFormat == ResponseFormat.Console;

    public string SendReport(Report report)
    {
        Console.WriteLine($"{"client_id", -10} {"order_sum", -10} {"favorite_item_name", -10}");
        foreach (var row in report.Rows)
        {
            Console.WriteLine($"{row.ClientId, -10} {row.OrderSum, -10} {row.FavoriteItemName, -10}");
        }
        return "Отчёт успешно отправлен в консоль приложения!";
    }
}