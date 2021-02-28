using Newtonsoft.Json;

namespace Dtos.TimeLineService
{
    public class NewsFeedTimeLineEntry
    {
        [JsonProperty("tweetId")]
        public long TweetId { get; set; }
    }
}