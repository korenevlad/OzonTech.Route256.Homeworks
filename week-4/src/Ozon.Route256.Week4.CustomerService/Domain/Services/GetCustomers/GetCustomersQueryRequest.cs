using MediatR;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

public sealed record GetCustomersQueryRequest : IRequest<GetCustomersQueryResponse>
{
    public long[] CustomerIds { get; }
    public long[] RegionIds { get; }
    public string[] FullNames { get; }

    public GetCustomersQueryRequest(long[] customerIds, long[] regionIds, string[] fullNames)
    {
        CustomerIds = customerIds;
        RegionIds = regionIds;
        FullNames = fullNames;
    }
}