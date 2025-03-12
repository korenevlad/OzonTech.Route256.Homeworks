namespace Ozon.Route256.Week4.CustomerService.Domain.Services.CreateCustomer;

public sealed class CreateCustomerCommandResponse : ResponseBase
{
    public long? CustomerId { get; }

    public CreateCustomerCommandResponse(long customerId)
    {
        CustomerId = customerId;
    }

    public CreateCustomerCommandResponse(Exception exception) : base(exception){ }
}