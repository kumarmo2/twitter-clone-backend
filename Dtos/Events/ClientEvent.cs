using System.Collections.Generic;

namespace Dtos.Events
{
    public class ClientEvent
    {
        public EventType EventType { get; set; }
        public Dictionary<string, object> EventData { get; set; }
    }
}