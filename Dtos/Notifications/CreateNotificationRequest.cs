namespace Dtos.Notifications
{
    public class CreateNotificationRequest
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public string Url { get; set; }
        public long Type { get; set; }
    }
}