using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using EventHubReceiver.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHubReceiver.Services
{
    public class EventHubConsumerService : IEventHubConsumerService
    {
        private readonly EventHubOptions _options;
        private readonly ILogger<EventHubConsumerService> _logger;

        public EventHubConsumerService(IOptions<EventHubOptions> options, ILogger<EventHubConsumerService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> ReceiveEvents()
        {
            List<string> events = new();

            EventHubConsumerClient consumerClient = new(
                EventHubConsumerClient.DefaultConsumerGroupName,
                _options.EventHubNamespace,
                _options.EventHubName,
                new Azure.Identity.ClientSecretCredential(
                    _options.TenantId,
                    _options.ClientId,
                    _options.ClientSecret
                ),
                new EventHubConsumerClientOptions
                {
                    ConnectionOptions = new EventHubConnectionOptions
                    {
                        TransportType = EventHubsTransportType.AmqpWebSockets
                    }
                });

            try
            {

                // Receive events from the event hub 
                await foreach (PartitionEvent partitionEvent in consumerClient.ReadEventsAsync())
                {
                    // Process the received event 
                    var @event = partitionEvent.Data.EventBody.ToString();
                    events.Add(@event);
                }

            }
            finally
            {
                // Close the consumer client 
                await consumerClient.CloseAsync();
            }

            return events;
        }
    }
}
