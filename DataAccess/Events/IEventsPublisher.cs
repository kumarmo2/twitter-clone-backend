using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos.Events;
using Models.Events;

namespace DataAccess.Events
{
    public interface IEventsPublisher
    {
        void PublishEvents(PublishEventRequest publishEventRequest, IEnumerable<UserQueue> userQueues);
    }
}