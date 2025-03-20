namespace Ozon.Route256.Week4.CustomerService.Domain.Models;

public record Customer
{
    public long Id { get; init; }
    public string FullName { get; init; }
    public long RegionId { get; init; }
    public DateTime CreatedAt { get; init; }
}