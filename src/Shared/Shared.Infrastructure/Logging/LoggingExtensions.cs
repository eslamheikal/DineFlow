using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Shared.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static IServiceCollection AddLogging(this IServiceCollection services, string serviceName)
    {
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog(dispose: true);
        });

        return services;
    }

} 