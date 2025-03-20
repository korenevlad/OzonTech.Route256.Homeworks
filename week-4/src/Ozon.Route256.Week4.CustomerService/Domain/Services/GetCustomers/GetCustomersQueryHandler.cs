using MediatR;
using Ozon.Route256.Week4.CustomerService.DAL.Repositories;
using Ozon.Route256.Week4.CustomerService.Extensions.Mapping;

namespace Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQueryRequest, GetCustomersQueryResponse>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<GetCustomersQueryResponse> Handle(GetCustomersQueryRequest request, CancellationToken cancellationToken)
    {
        var queryResult = await _customerRepository.GetCustomers(
            request.CustomerIds,
            request.RegionIds,
            request.FullNames,
            cancellationToken);
        return new GetCustomersQueryResponse(queryResult.ToBll(), queryResult.Length);
    }
}