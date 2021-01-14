using System;

namespace TwitterWeb.Models.Tweets
{
    public class Like
    {
        public long Id { get; set; }
        public long TweetId { get; set; }
        public long UserId { get; set; } //The one who liked
        public DateTime CreatedAt { get; set; }
    }
}