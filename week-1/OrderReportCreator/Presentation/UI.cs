using System.Text;
using OrderReportCreator.Exceptions;
using OrderReportCreator.Requests;

namespace OrderReportCreator.Presentation;
public class UI: IUI
{
    public Request GetRequest()
    {
        var reportFormat = GetReportFormat();
        var clientIds = GetClientIds();
        return new Request()
        {
            ResponseFormat = reportFormat == "console" ? ResponseFormat.Console : ResponseFormat.File,  
            Ids = clientIds 
        };
    }
    
    private string GetReportFormat()
    {
        Console.InputEncoding = Encoding.UTF8;
        using var streamReader = new StreamReader(Console.OpenStandardInput());
        WriteConsole("Введите формат отчёта.\n" +
                     "Если вы хотите, чтобы отчёт был выведен в консоль - введите  \"console\".\n" +
                     "Если вы хотите, чтобы отчёт был сохранён в файл - введите  \"file\".");
        var reportFormat = streamReader.ReadLine();
        Console.WriteLine();
        if (reportFormat != "console" && reportFormat != "file")
        {
            throw new IncorrectReportFormatException(reportFormat);
        }
        return reportFormat;
    }

    private List<long> GetClientIds()
    {
        Console.InputEncoding = Encoding.UTF8;
        using var streamReader = new StreamReader(Console.OpenStandardInput());
        WriteConsole("Введите через пробел уникальные id клиентов для составления отчета:");
        var clientIdsStr = streamReader.ReadLine();
        Console.WriteLine();
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

    private void WriteConsole(string message)
    {
        Console.OutputEncoding = Encoding.UTF8;
        using var streamWriter = new StreamWriter(Console.OpenStandardOutput());
        streamWriter.WriteLine(message);
        streamWriter.Flush();
    }
}