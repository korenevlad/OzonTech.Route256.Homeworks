namespace Ozon.Route256.Week4.CustomerService.Domain.Services.DeleteCustomersByIds;

public sealed class DeleteCustomersIdsCommandResponse : ResponseBase
{
    public long[]? CustomerIds { get; }
    
    public DeleteCustomersIdsCommandResponse(long[] customerIds)
    {
        CustomerIds = customerIds;
    }

    public DeleteCustomersIdsCommandResponse(Exception exception) : base(exception){ }
}