namespace EventHubReceiver.Configuration
{
    public class EventHubOptions
    {
        public const string ConfigurationSectionName = "EventHub";

        public string EventHubNamespace { get; set; }
        public string EventHubName { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
