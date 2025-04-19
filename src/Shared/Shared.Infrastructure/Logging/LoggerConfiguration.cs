using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Shared.Infrastructure.Logging;

public static class LoggerConfiguration
{
    public static IHostBuilder AddBuilderLogging(this IHostBuilder builder, string serviceName)
    {
        return builder.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProperty("ServiceName", serviceName)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId} {Message:lj}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code)
            .WriteTo.File(
                path: $"logs/{serviceName}/log-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            //{
            //    AutoRegisterTemplate = true,
            //    IndexFormat = $"{serviceName}-logs-{context.HostingEnvironment.EnvironmentName}-{DateTime.UtcNow:yyyy-MM}",
            //    CustomFormatter = new ElasticsearchJsonFormatter()
            //})
            );
    }
} 