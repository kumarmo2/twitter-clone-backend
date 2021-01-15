namespace Models.Events
{
    public class UserQueue
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string QueueName { get; set; }
    }
}