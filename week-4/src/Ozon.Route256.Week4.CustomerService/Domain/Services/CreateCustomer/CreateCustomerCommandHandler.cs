using MediatR;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories.Exceptions;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommandRequest, CreateCustomerCommandResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommandRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var customerId = await _customerRepository.CreateCustomer(request.FullName, request.RegionId, cancellationToken);
            return new CreateCustomerCommandResponse(customerId);
        }
        catch (Exception ex)
        {
            if (ex is CustomerAlreadyExistsException)
            {
                return new CreateCustomerCommandResponse(ex);
            }

            throw;
        }
    }
}