using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Shared.Infrastructure.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string HeaderKey = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(HeaderKey, out var correlationId))
            correlationId = Guid.NewGuid().ToString();

        context.TraceIdentifier = correlationId;
        context.Request.Headers[HeaderKey] = correlationId;

        using (LogContext.PushProperty("CorrelationId", string.IsNullOrEmpty(correlationId) ? "" : $" [TraceId:{correlationId}]"))
        {
            await _next(context);
        }
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CorrelationIdMiddleware>();
    }
}