using MediatR;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.DeleteCustomersByIds;

public class DeleteCustomersByIdsCommandHandler : IRequestHandler<DeleteCustomersByIdsCommandRequest, DeleteCustomersIdsCommandResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public DeleteCustomersByIdsCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<DeleteCustomersIdsCommandResponse> Handle(DeleteCustomersByIdsCommandRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _customerRepository.DeleteCustomers(request.CustomerIds, cancellationToken);

            return new DeleteCustomersIdsCommandResponse(request.CustomerIds);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}