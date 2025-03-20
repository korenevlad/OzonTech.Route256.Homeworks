using MediatR;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.CreateCustomer;

public sealed record CreateCustomerCommandRequest : IRequest<CreateCustomerCommandResponse>
{
    public string FullName { get; }
    public long RegionId { get; }
    
    public CreateCustomerCommandRequest(string fullName, long regionId)
    {
        FullName = fullName;
        RegionId = regionId;
    }
}