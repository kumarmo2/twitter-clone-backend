using System.Threading.Tasks;
using Business.Events;
using Dtos.Events;
namespace EventsConsumer.Controllers
{
    public class EventsController
    {
        private readonly IEventsLogic _eventsLogic;
        public EventsController(IEventsLogic eventsLogic)
        {
            _eventsLogic = eventsLogic;
        }
        public async Task ProcessEvent(PublishEventRequest publishEventRequest)
        {
            if (publishEventRequest is null)
            {
                throw new System.ArgumentNullException(nameof(publishEventRequest));
            }

            await _eventsLogic.PublishEvent(publishEventRequest);
        }
    }
}