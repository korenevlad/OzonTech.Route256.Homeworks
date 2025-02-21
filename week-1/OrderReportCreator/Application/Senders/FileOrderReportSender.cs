using System.Globalization;
using CsvHelper;
using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Senders;
public class FileOrderReportSender: IOrderReportSender
{
    private const string DirPath = "ReportFiles";
    public bool CanSendReport(ResponseFormat responseFormat) 
        => responseFormat == ResponseFormat.File;
    public string SendReport(Report report)
    {
        var fullPathDir = GetPathDir();
        try
        {
            if (!Directory.Exists(fullPathDir))
            {
                Directory.CreateDirectory(fullPathDir);
            }
            var fileName = $"Report-{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var filePath = Path.Combine(fullPathDir, fileName);
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
    private string GetProjectRoot()
    {
        var baseDirPath = new DirectoryInfo(AppContext.BaseDirectory);
        while (baseDirPath != null && !File.Exists(Path.Combine(baseDirPath.FullName, $"{baseDirPath.Name}.csproj")))
        {
            baseDirPath = baseDirPath.Parent;
        }
        return baseDirPath?.FullName ?? throw new Exception("Не удалось найти корень проекта");
    }

    private string GetPathDir()
    {
        var projectRoot = GetProjectRoot();
        string relativePathDir = Path.Combine(projectRoot, "..", DirPath);
        string fullPathDir = Path.GetFullPath(relativePathDir);
        return fullPathDir;
    }
}