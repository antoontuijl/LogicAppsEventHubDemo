using EventHubReceiver.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventHubReceiver.Tests
{
    public class EventsTests
    {
        [Fact]
        public void GivenJson_ShouldDeserialze_ToAanvraagEvent()
        {
            var json = File.ReadAllText("Events/Aanvraag.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            AanvraagEvent? @event = JsonSerializer.Deserialize<AanvraagEvent>(json, options);

            Assert.NotNull(@event);
        }
    }
}