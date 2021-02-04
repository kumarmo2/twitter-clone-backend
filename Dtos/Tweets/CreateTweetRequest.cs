using System;

namespace Dtos.Tweets
{
    public class CreateTweetRequest
    {
        public long AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string Content { get; set; }
        public long? ParentTweetId { get; set; }
        public long? QuotedTweetId { get; set; }
        public long? RetweetedTweetId { get; set; }
    }
}