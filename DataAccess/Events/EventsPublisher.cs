using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos.Events;
using Models.Events;
using Utils.Common;

namespace DataAccess.Events
{
    public class EventsPublisher : IEventsPublisher
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        public EventsPublisher(IRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }
        public void PublishEvents(PublishEventRequest publishEventRequest, IEnumerable<UserQueue> userQueues)
        {
            var allTasks = new List<Task>();

            using (var channel = _rabbitMqClient.GetChannel())
            {
                var clientEvent = GetClientEventFromPublishEventRequest(publishEventRequest);
                foreach (var userQueue in userQueues)
                {
                    _rabbitMqClient.PushToQueue(userQueue.QueueName, clientEvent);
                }
            }
        }

        private static ClientEvent GetClientEventFromPublishEventRequest(PublishEventRequest publishEventRequest) => new ClientEvent
        {
            EventData = publishEventRequest.EventData,
            EventType = publishEventRequest.EventType
        };
    }
}