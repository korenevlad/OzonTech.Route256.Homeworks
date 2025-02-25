namespace OrderReportCreator.Requests;

public record GenerateOrderReportRequest
{
    public ResponseFormat ResponseFormat { get; init; }
    public IEnumerable<long> Ids { get; init; }
}