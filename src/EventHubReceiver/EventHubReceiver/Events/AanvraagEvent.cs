using EventHubReceiver.Events;
using System.Text.Json;

namespace EventHubReceiver.Events
{
    public class AanvraagEvent
    {
        public required Aanvraag Aanvraag { get; set; }
        public required string Operation { get; set; }
    }

    public class Aanvraag
    {
        public required Aansluitingsobjecten[] AansluitingsObjecten { get; set; }
        public required int AanvraagId { get; set; }
    }

    public class Aansluitingsobjecten
    {
        public required int[] DisciplineIds { get; set; }
        public required int ObjectId { get; set; }
    }
}

public static class AanvraagEventExtensions
{
    public static AanvraagEvent? ToAanvraagEvent(this string json)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        return JsonSerializer.Deserialize<AanvraagEvent>(json, options);
    }
}

