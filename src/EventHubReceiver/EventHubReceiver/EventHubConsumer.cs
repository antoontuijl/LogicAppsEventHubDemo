using EventHubReceiver.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EventHubReceiver
{
    public class EventHubConsumer
    {
        private readonly IEventHubConsumerService _eventHubConsumerService;
        private readonly ILogger<EventHubConsumer> _logger;

        public EventHubConsumer(IEventHubConsumerService eventHubConsumerService, ILogger<EventHubConsumer> logger)
        {
            _eventHubConsumerService = eventHubConsumerService;
            _logger = logger;
        }

        [Function(nameof(EventHubConsumer))]
        //[ServiceBusOutput("lip.onramp.aansluitaanvraag", Connection = "ServiceBusConnection")]
        //[QueueOutput("lip.onramp.aansluitaanvraag")]
        public async Task RunAsync([TimerTrigger("* * * * *")] TimerInfo myTimer)
        {
            // Receive events
            using CancellationTokenSource cancellationSource = new();
            var events = await _eventHubConsumerService.ReceiveEvents(cancellationSource);

            foreach (var @event in events)
            {

                
                // Post event to Queue.
            }
        }
    }
}
