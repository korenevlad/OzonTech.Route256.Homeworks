using System.Text;
using OrderReportCreator.Exceptions;
using OrderReportCreator.Requests;

namespace OrderReportCreator.Presentation;
public class UI: IUI
{
    public GenerateOrderReportRequest GetRequest()
    {
        var reportFormat = GetReportFormat();
        var clientIds = GetClientIds();
        return new GenerateOrderReportRequest()
        {
            ResponseFormat = reportFormat == "console" ? ResponseFormat.Console : ResponseFormat.File,  
            Ids = clientIds 
        };
    }
    
    private string GetReportFormat()
    {
        SendMessage("Введите формат отчёта. " +
                     "Если вы хотите, чтобы отчёт был выведен в консоль - введите  \"console\". " +
                     "Если вы хотите, чтобы отчёт был сохранён в файл - введите  \"file\".");
        Console.InputEncoding = Encoding.UTF8;
        using var streamReader = new StreamReader(Console.OpenStandardInput());
        var reportFormat = streamReader.ReadLine();
        if (reportFormat != "console" && reportFormat != "file")
        {
            throw new IncorrectReportFormatException(reportFormat);
        }
        return reportFormat;
    }

    private IEnumerable<long> GetClientIds()
    {
        SendMessage("Введите через пробел уникальные id клиентов для составления отчета:");
        Console.InputEncoding = Encoding.UTF8;
        using var streamReader = new StreamReader(Console.OpenStandardInput());
        var clientIdsStr = streamReader.ReadLine();
        if (string.IsNullOrWhiteSpace(clientIdsStr))
        {
            throw new EmptyClientIdListException();
        }
        var clientIds = clientIdsStr.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToHashSet();
        var clientIdsToRequest = clientIds
            .Select(clientId =>
            {
                if (long.TryParse(clientId, out var result)) return result;
                else throw new ClientIdConversionException(clientId);
            })
            .ToList();
        return clientIdsToRequest;
    }

    public void SendMessage(string message)
    {
        Console.OutputEncoding = Encoding.UTF8;
        using var streamWriter = new StreamWriter(Console.OpenStandardOutput());
        streamWriter.WriteLine(message);
        streamWriter.Flush();
    }
}