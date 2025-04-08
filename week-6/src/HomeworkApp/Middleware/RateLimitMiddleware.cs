using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace HomeworkApp.Middleware;
public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConnectionMultiplexer _redis;
    private const int RequestLimit = 100;
    private const int TimeInSeconds = 60;

    public RateLimitMiddleware(RequestDelegate next, ConnectionMultiplexer redis)
    {
        _next = next;
        _redis = redis;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string userIp = context.Request.Headers["X-R256-USER-IP"];
        string key = $"rate_limit:{userIp}";
        
        var db = _redis.GetDatabase();
        var currentRequestCount = await db.StringIncrementAsync(key);

        if (currentRequestCount > RequestLimit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return;
        }
        
        if (currentRequestCount == 1)
            await db.KeyExpireAsync(key, TimeSpan.FromSeconds(TimeInSeconds));
        
        await _next(context);
    }
}