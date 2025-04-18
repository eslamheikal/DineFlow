using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;

namespace Shared.Infrastructure.Logging;

public class StructuredLogger<T>
{
    private readonly ILogger<T> _logger;
    private readonly string _serviceName;

    public StructuredLogger(ILogger<T> logger, string serviceName)
    {
        _logger = logger;
        _serviceName = serviceName;
    }

    public void LogOperation(string operation, Action action)
    {
        var operationId = Guid.NewGuid().ToString();
        var stopwatch = Stopwatch.StartNew();

        using (LogContext.PushProperty("OperationId", operationId))
        using (LogContext.PushProperty("ServiceName", _serviceName))
        {
            try
            {
                _logger.LogInformation("Starting operation {Operation} in {Service}", operation, _serviceName);

                action();

                stopwatch.Stop();
                _logger.LogInformation("Completed operation {Operation} in {Service} in {Elapsed}ms", 
                    operation, _serviceName, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Failed operation {Operation} in {Service} after {Elapsed}ms", 
                    operation, _serviceName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    public async Task LogOperationAsync(string operation, Func<Task> action)
    {
        var operationId = Guid.NewGuid().ToString();
        var stopwatch = Stopwatch.StartNew();

        using (LogContext.PushProperty("OperationId", operationId))
        using (LogContext.PushProperty("ServiceName", _serviceName))
        {
            try
            {
                _logger.LogInformation("Starting operation {Operation} in {Service}", operation, _serviceName);

                await action();

                stopwatch.Stop();
                _logger.LogInformation("Completed operation {Operation} in {Service} in {Elapsed}ms",
                    operation, _serviceName, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Failed operation {Operation} in {Service} after {Elapsed}ms",
                    operation, _serviceName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    public async Task<TResult> LogOperationAsync<TResult>(string operation, Func<Task<TResult>> action)
    {
        var operationId = Guid.NewGuid().ToString();
        var stopwatch = Stopwatch.StartNew();

        using (LogContext.PushProperty("OperationId", operationId))
        using (LogContext.PushProperty("ServiceName", _serviceName))
        {
            try
            {
                _logger.LogInformation("Starting operation {Operation} in {Service}", operation, _serviceName);

                var result = await action();

                stopwatch.Stop();
                _logger.LogInformation("Completed operation {Operation} in {Service} in {Elapsed}ms",
                    operation, _serviceName, stopwatch.ElapsedMilliseconds);

                return result;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Failed operation {Operation} in {Service} after {Elapsed}ms",
                    operation, _serviceName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }

    public void LogEvent(string eventName, Dictionary<string, object>? properties = null)
    {
        var eventId = Guid.NewGuid().ToString();

        using (LogContext.PushProperty("EventId", eventId))
        using (LogContext.PushProperty("ServiceName", _serviceName))
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    LogContext.PushProperty(prop.Key, prop.Value);
                }
            }

            _logger.LogInformation("Event {EventName} in {Service}", eventName, _serviceName);
        }
    }
}