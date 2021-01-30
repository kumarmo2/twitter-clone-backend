using System.Collections.Generic;

namespace Dtos.Events
{
    public class PublishEventRequest
    {
        public long UserId { get; set; }
        public EventType EventType { get; set; }
        public Dictionary<string, object> EventData { get; set; }
    }
}