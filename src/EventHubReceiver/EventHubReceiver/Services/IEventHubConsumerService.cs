
namespace EventHubReceiver.Services
{
    public interface IEventHubConsumerService
    {
        Task<IEnumerable<string>> ReceiveEvents();
    }
}