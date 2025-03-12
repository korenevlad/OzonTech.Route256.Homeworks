using MediatR;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

public sealed record GetCustomersQueryRequest : IRequest<GetCustomersQueryResponse>
{
    public long[] CustomerIds { get; init; }
    public long[] RegionIds { get; set; }
    public string[] FullNames { get; set; }

    public GetCustomersQueryRequest(long[] customerIds, long[] regionIds, string[] fullNames)
    {
        CustomerIds = customerIds;
        RegionIds = regionIds;
        FullNames = fullNames;
    }
}