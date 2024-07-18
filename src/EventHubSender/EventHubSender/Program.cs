using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;
using System.Text;
using Azure.Identity;

namespace EventHubSender
{
    class Program
    {
        private const string eventHubNamespaceFqdn = "lip-aanvragen-eh-dev.servicebus.windows.net";  // Replace with your Event Hub namespace FQDN
        private const string eventHubName = "aanvragen";  // Replace with your Event Hub name

        static async Task Main(string[] args)
        {
            // Create a producer client that you can use to send events to an Event Hub
            var producerClient = new EventHubProducerClient(eventHubNamespaceFqdn, eventHubName, new DefaultAzureCredential());

            // Create a batch of events 
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

            // Add events to the batch. Only one event is being added in this example.
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Hello, Event Hub!")));

            // Use the producer client to send the batch of events to the Event Hub
            await producerClient.SendAsync(eventBatch);

            Console.WriteLine("A batch of events has been published.");

            // Dispose of the producer client
            await producerClient.DisposeAsync();
        }
    }
}