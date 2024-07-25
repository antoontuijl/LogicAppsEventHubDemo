using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
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

        public async Task<IEnumerable<string>> ReceiveEvents(CancellationTokenSource cancellationSource)
        {
            List<string> events = new();
            
            var storageClient =
                new BlobContainerClient(_options.CheckpointConnectionString, _options.CheckpointContainer);

            var processor = new EventProcessorClient(
                storageClient,
                EventHubConsumerClient.DefaultConsumerGroupName,
                _options.EventHubConnectionString);
            
            try
            {
                processor.ProcessEventAsync += ProcessEventHandler;
                processor.ProcessErrorAsync += ProcessErrorHandler;

                try
                {
                    await processor.StartProcessingAsync(cancellationSource.Token);
                    await Task.Delay(TimeSpan.FromSeconds(_options.EventHubReceiverWaitTimeInSeconds));
                }
                catch (TaskCanceledException)
                {
                    // This is expected if the cancellation token is
                    // signaled.
                }
                finally
                {
                    await processor.StopProcessingAsync(cancellationSource.Token);
                }
            }
            finally
            {
                processor.ProcessEventAsync -= ProcessEventHandler;
                processor.ProcessErrorAsync -= ProcessErrorHandler;
            }

            async Task ProcessEventHandler(ProcessEventArgs eventArgs)
            {
                string @event = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
                events.Add(@event);
                
                await eventArgs.UpdateCheckpointAsync(cancellationSource.Token);
            }

            Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
            {
                _logger.LogCritical($"\tPartition '{eventArgs.PartitionId}': an unhandled exception was encountered. This was not expected to happen.");

                return Task.CompletedTask;
            }
        
            return events;
        }
    }
}
