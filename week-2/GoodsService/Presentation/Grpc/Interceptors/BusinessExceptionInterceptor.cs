using GoodsService.BLL.Exceptions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace GoodsService.Presentation.Grpc.Interceptors;
public class BusinessExceptionInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (BusinessException ex)
        {
            var status = new Status(StatusCode.FailedPrecondition, $"{ex.Message}");
            throw new RpcException(status, $"{ex.Message}");
        }
    }
}