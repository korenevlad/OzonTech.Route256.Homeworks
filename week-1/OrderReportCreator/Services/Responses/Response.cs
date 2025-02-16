using OrderReportCreator.Domain.Models;

namespace OrderReportCreator;
public record Response
{
    //TODO: тут надо отдельно модельку на Report, чтоб потом автомапить наверна
    public IEnumerable<Report> ResponseItems { get; init; }
}