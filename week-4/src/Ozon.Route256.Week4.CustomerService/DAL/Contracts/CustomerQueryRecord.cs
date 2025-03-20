namespace Ozon.Route256.Week4.CustomerService.DAL.Contracts;

public record CustomerDbRecord
{
    public long Id { get; init; }
    public required string FullName { get; init; }
    public long RegionId { get; init; }
    public DateTime CreatedAt { get; init; }
}