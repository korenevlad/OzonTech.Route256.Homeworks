using Grpc.Core.Interceptors;
using Grpc.Core;
using System.Text.Json;

namespace GoodsService.Presentation.Grpc.Interceptors;
public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context, 
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var requestJson = JsonSerializer.Serialize(request);
            _logger.LogInformation("Request: {Request}", requestJson);

            var response = await continuation(request, context);

            var responseJson = JsonSerializer.Serialize(response);
            _logger.LogInformation("Response: {Response}", responseJson);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла ошибка обработки запроса!");
            throw;
        }
    }
}
