using Moq;
using StackExchange.Redis;
using Microsoft.AspNetCore.Http;
using HomeworkApp.Middleware;
using Xunit;

namespace HomeworkApp.UnitTests;
public class RateLimitMiddlewareTests
{
    [Fact]
    public async Task TestRateLimit_WithinLimit()
    {
        // Arrange
        var configuration = ConfigurationOptions.Parse("localhost:16379");
        var redis = ConnectionMultiplexer.Connect(configuration);
        var db = redis.GetDatabase();

        var middleware = new RateLimitMiddleware((innerHttpContext) => Task.CompletedTask, redis);
        var context = new DefaultHttpContext();
        context.Request.Headers["X-R256-USER-IP"] = "127.0.0.1";

        await db.KeyDeleteAsync("rate_limit:127.0.0.1");
        await db.StringSetAsync("rate_limit:127.0.0.1", 0);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(200, context.Response.StatusCode);
    }


    [Fact]
    public async Task TestRateLimit_ExceededLimit()
    {
        // Arrange
        var configuration = ConfigurationOptions.Parse("localhost:16379");
        var redis = ConnectionMultiplexer.Connect(configuration);
        var db = redis.GetDatabase();

        var middleware = new RateLimitMiddleware((innerHttpContext) => Task.CompletedTask, redis);
        var context = new DefaultHttpContext();
        context.Request.Headers["X-R256-USER-IP"] = "127.0.0.1";

        await db.KeyDeleteAsync("rate_limit:127.0.0.1");
        await db.StringSetAsync("rate_limit:127.0.0.1", 101.0);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(429, context.Response.StatusCode);
    }

}