using System.Globalization;
using CsvHelper;
using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Senders;
public class FileReportSender: IReportSender
{
    private const string DirPath = @"..\..\..\..\ReportFiles";
    public bool CanSendReport(ResponseFormat responseFormat) 
        => responseFormat == ResponseFormat.File;
    public string SendReport(Report report)
    {
        try
        {
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }
            var fileName = $"Report-{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(DirPath, fileName);
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(report.Rows);
            }
            return $"Отчёт успешно отправлен! Просмотреть отчёт можно в {filePath} ";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}