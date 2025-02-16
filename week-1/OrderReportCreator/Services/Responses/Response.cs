using OrderReportCreator.Domain.Models;

namespace OrderReportCreator;
public record Response
{
    //TODO: тут надо отдельно модельку на ReportItem, чтоб потом автомапить наверна
    public IEnumerable<ReportItem> ResponseItems { get; init; }
}