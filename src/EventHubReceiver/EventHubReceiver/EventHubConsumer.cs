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
        public async Task RunAsync([TimerTrigger("%EventHubConsumerCron%")] TimerInfo myTimer)
        {
            // Receive events
            var events = await _eventHubConsumerService.ReceiveEvents();

            foreach (var @event in events)
            {

                
                // Post event to Queue.
            }
        }
    }
}
