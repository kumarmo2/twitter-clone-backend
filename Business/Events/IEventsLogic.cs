using System.Threading.Tasks;
using Dtos;
using Models.Events;
using Dtos.Events;

namespace Business.Events
{
    public interface IEventsLogic
    {
        // Events Web Starts /////////
        Task<Result<UserQueue>> RegisterUserEvent(long userId);
        Task<Result<ClientEvent>> GetEvents(long userId, string queueName);

        // Events Service Ends /////////

        // This is called by any service that wants send events to the clients.
        // This in turns just push to the exchange where it will be picked up by the `Events Service`
        // again to acutally push to the user queues.
        // This is done for reasons:
        //  - So as to hide the implementation of publishing the events i.e to hide the fact that the events
        // sit behind an exchange/queue. If in future we decide to eradicate the message broker or use different broker, clients will
        // not need to make any changes.
        void CreateClientEvent(PublishEventRequest publishEventRequest);

        // `PublishEvent` publishes events user queues, where it will be picked by `Events Web Service`
        Task PublishEvent(PublishEventRequest publishEventRequest);

        // Events Service Ends/////
    }
}