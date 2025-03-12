using MediatR;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.DeleteCustomersByIds;

public sealed record DeleteCustomersByIdsCommandRequest : IRequest<DeleteCustomersIdsCommandResponse>
{
    public long[] CustomerIds { get; }

    public DeleteCustomersByIdsCommandRequest(long[] customerIds)
    {
        CustomerIds = customerIds;
    }

}