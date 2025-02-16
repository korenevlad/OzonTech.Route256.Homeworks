using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application.Commands;
public interface IOrderReportService
{
    IEnumerable<Report> CreateOrderReports();
}