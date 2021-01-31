namespace Dtos.Notifications
{
    public class Notification
    {
        public string Content { get; set; }
        public string Url { get; set; }
        public bool IsSeen { get; set; }
        public long Type { get; set; }
    }
}