using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Ozon.Route256.Week4.CustomerService.Domain.Services.CreateCustomer;
using Ozon.Route256.Week4.CustomerService.Domain.Services.DeleteCustomersByIds;
using Ozon.Route256.Week4.CustomerService.Domain.Services.GetCustomers;

namespace Ozon.Route256.Week4.CustomerService.Presentation.Controllers.Grpc;

public sealed class CustomerController : CustomerService.CustomerServiceBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator  mediator)
    {
        _mediator = mediator;
    }

    public override async Task<V1CreateCustomerResponse> V1CreateCustomer(V1CreateCustomerRequest request, ServerCallContext context)
    {
        var internalRequest = new CreateCustomerCommandRequest(request.FullName, request.RegionId);
        var internalResponse = await _mediator.Send(internalRequest, context.CancellationToken);
        
        return internalResponse.Successful
            ? new V1CreateCustomerResponse { Ok = new V1CreateCustomerResponse.Types.CreateSuccess { CustomerId = internalResponse.CustomerId!.Value } }
            : new V1CreateCustomerResponse { Error = new V1CreateCustomerResponse.Types.CreateError { Code = internalResponse.Exception!.GetType().Name,  Text = internalResponse.Exception.Message } };
    }
    
    public override async Task<V1DeleteCustomersByIdsResponse> V1DeleteCustomersByIds(V1DeleteCustomersByIdsRequest request,
        ServerCallContext context)
    {
        var internalRequest = new DeleteCustomersByIdsCommandRequest(request.CustomerIds.ToArray());
        var internalResponse = await _mediator.Send(internalRequest, context.CancellationToken);

        return internalResponse.Successful
            ? new V1DeleteCustomersByIdsResponse { Ok = new V1DeleteCustomersByIdsResponse.Types.DeleteSuccess { CustomerIds = { internalResponse.CustomerIds! }} }
            : new V1DeleteCustomersByIdsResponse { Error = new V1DeleteCustomersByIdsResponse.Types.DeleteError { Code = internalResponse.Exception!.GetType().Name,  Text = internalResponse.Exception.Message } };
    }

    public override async Task<V1QueryCustomersResponse> V1QueryCustomers(V1QueryCustomersRequest request,
        ServerCallContext context)
    {
        var internalRequest = new GetCustomersQueryRequest(request.CustomerIds.ToArray(), request.RegionIds.ToArray(),
            request.FullNames.ToArray());
        var internalResponse = await _mediator.Send(internalRequest, context.CancellationToken);

        return new V1QueryCustomersResponse
        {
            TotalCount = internalResponse.TotalCount,
            Customers =
            {
                internalResponse.Customers.Select(x => new V1QueryCustomersResponse.Types.Customer
                {
                    CustomerId = x.Id,
                    FullName = x.FullName,
                    RegionId = x.RegionId,
                    CreatedAt = x.CreatedAt.ToTimestamp()
                })
            }
        };
    }
}

