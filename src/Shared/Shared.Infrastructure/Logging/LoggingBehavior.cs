using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace Shared.Infrastructure.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace";
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Begin {RequestName} - TraceId: {TraceId} - Payload: {@Request}",
                requestName, traceId, SanitizeRequest(request));

            var response = await next();

            stopwatch.Stop();
            _logger.LogInformation(
                "Completed {RequestName} in {ElapsedMs}ms - TraceId: {TraceId}",
                requestName, stopwatch.ElapsedMilliseconds, traceId);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error {RequestName} in {ElapsedMs}ms - TraceId: {TraceId}",
                requestName, stopwatch.ElapsedMilliseconds, traceId);

            throw;
        }
    }

    private static object SanitizeRequest(TRequest request)
    {
        if (request == null) return null;

        // Create anonymous object with sensitive data redacted
        var sanitized = new Dictionary<string, object>();
        var properties = typeof(TRequest).GetProperties();

        foreach (var prop in properties)
        {
            var value = prop.GetValue(request);
            sanitized[prop.Name] = prop.GetCustomAttribute<IgnoreLoggingAttribute>() != null
                ? "***REDACTED***"
                : value;
        }

        return sanitized;
    }
}