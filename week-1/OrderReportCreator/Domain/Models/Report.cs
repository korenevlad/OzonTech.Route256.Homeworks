namespace OrderReportCreator.Domain.Models;

public class Report
{
    public List<ReportRow> Rows { get; set; } = new();
}