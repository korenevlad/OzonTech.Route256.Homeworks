using OrderReportCreator.Domain.Models;
using OrderReportCreator.Requests;

namespace OrderReportCreator.Application;
public interface IOrderReportService
{
    public IEnumerable<Report> CreateOrderReport(Request request);
}