namespace Models.Notifications
{
    public class Notification
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }

        public string Url { get; set; }
        public bool IsSeen { get; set; }
        public long Type { get; set; }
    }
}