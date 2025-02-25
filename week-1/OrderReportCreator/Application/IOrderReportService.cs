using OrderReportCreator.Domain.Models;

namespace OrderReportCreator.Application;
public interface IOrderReportService
{
    public Report CreateOrderReport(IEnumerable<long> ids);
}