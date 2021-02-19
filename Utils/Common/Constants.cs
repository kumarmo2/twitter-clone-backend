namespace Utils.Common
{
    public static class Constants
    {
        public const string JwtUserIdClaimKey = "userId";
        public const string AuthCookieName = "auth";
        public const string AuthenticatedUserKey = "authenticateduser";
        public const string JwtSecret = "kumarmo2";
        public const string UserEventsExchangeName = "users.user-events";
        public const string UserEventsConsumerQueue = "users.user-consumer-queue";
        public const string EventsExchangeName = "events.events-exchange";
        public const string EventsQueueName = "events.events-consumer-queue";
        public const string TweetsExchangeName = "tweets.tweets-events";
        public const string TimeLineServiceConsumerQueue = "tweets.tweets-consumer-queue";
    }
}