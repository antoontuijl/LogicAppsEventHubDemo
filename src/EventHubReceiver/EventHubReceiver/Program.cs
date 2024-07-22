using EventHubReceiver.Configuration;
using EventHubReceiver.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddOptions<EventHubOptions>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(EventHubOptions.ConfigurationSectionName).Bind(settings);
        });

        services.AddScoped<IEventHubConsumerService, EventHubConsumerService>();
    })
    .Build();

host.Run();
