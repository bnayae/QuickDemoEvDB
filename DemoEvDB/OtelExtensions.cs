using EvDb.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.DependencyInjection;

internal static class OtelExtensions
{
    private const string APP_NAME = "funds:atm";


    public static WebApplicationBuilder AddOtel(this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.Configuration;

        #region Logging

        ILoggingBuilder loggingBuilder = builder.Logging;

        loggingBuilder.AddOpenTelemetry(logging =>
        {
            var resource = ResourceBuilder.CreateDefault();
            logging.SetResourceBuilder(resource.AddService(APP_NAME));
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.AddOtlpExporter()
                        .AddOtlpExporter("aspire", o => o.Endpoint = new Uri("http://localhost:18889"));
        });

        loggingBuilder.Configure(x =>
        {
            x.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
              | ActivityTrackingOptions.TraceId
              | ActivityTrackingOptions.Tags;
        });

        #endregion // Logging}

        var services = builder.Services;
        services.AddOpenTelemetry()
                    .ConfigureResource(resource =>
                                   resource.AddService(APP_NAME,
                                                    serviceInstanceId: "console-app",
                                                    autoGenerateServiceInstanceId: false)) // builder.Environment.ApplicationName
            .WithTracing(tracing =>
            {
                tracing
                        .AddEvDbInstrumentation()
                        .AddEvDbStoreInstrumentation()
                        .AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources")
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .SetSampler<AlwaysOnSampler>()
                        .AddOtlpExporter("aspire", o => o.Endpoint = new Uri("http://localhost:18889"));
            })
            .WithMetrics(meterBuilder =>
                    meterBuilder.AddEvDbInstrumentation()
                                .AddEvDbStoreInstrumentation()
                                .AddMeter("MongoDB.Driver.Core.Extensions.DiagnosticSources")
                                .AddAspNetCoreInstrumentation()
                                .AddHttpClientInstrumentation()
                                .AddOtlpExporter("aspire", o => o.Endpoint = new Uri("http://localhost:18889")));

        return builder;
    }

}
