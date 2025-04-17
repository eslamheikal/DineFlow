using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
                requestName, traceId, request);

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
}