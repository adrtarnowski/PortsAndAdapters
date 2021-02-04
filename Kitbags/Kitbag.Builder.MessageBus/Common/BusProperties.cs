namespace Kitbag.Builder.MessageBus.Common
{
    public class BusProperties
    {
        public string? ConnectionString { get; set; }

        public bool? AutoComplete { get; set; }

        public int? MaxConcurrentCalls { get; set; }

        public int? CommandMaxConcurrentCalls { get; set; }

        public int? MaxAutoRenewMinutesDuration { get; set; }

        public int? MessageWaitTimeoutInSeconds { get; set; }

        public string? EventTopicName { get; set; }
        
        public string? EventSubscriptionName { get; set; }
    }
}