namespace OrderReportCreator.Requests;

public record Request
{
    public ResponseFormat ResponseFormat { get; init; }
    public IEnumerable<long> Ids { get; init; }
}