
using System;

namespace Dtos.Users
{
    public class UserEvent
    {
        public UserEventType EventType { get; set; }
        public long FollowId { get; set; }
    }
}