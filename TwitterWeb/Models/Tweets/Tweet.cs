using System;

namespace TwitterWeb.Models.Tweets
{
    public class Tweet
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public long AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}