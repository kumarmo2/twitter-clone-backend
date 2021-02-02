using System;
using System.Threading.Tasks;
using DataAccess.Events;
using Dtos;
using Dtos.Events;
using Models.Events;
using Utils;
using Utils.Common;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace Business.Events
{
    public class EventsLogic : IEventsLogic
    {
        private readonly IIdentityFactory _identityFactory;
        private readonly IUserQueueRepository _userQueueRepository;
        private readonly IRabbitMqClient _rabbitMqClient;
        private readonly IEventsPublisher _eventsPublisher;
        private readonly EventOptions _eventOptions;
        public EventsLogic(IIdentityFactory identityFactory, IUserQueueRepository userQueueRepository,
            IRabbitMqClient rabbitMqClient, IOptions<EventOptions> eventOptions, IEventsPublisher eventsPublisher)
        {
            _identityFactory = identityFactory;
            _userQueueRepository = userQueueRepository;
            _rabbitMqClient = rabbitMqClient;
            _eventOptions = eventOptions?.Value;
            _eventsPublisher = eventsPublisher;
        }
        public async Task<Result<UserQueue>> RegisterUserEvent(long userId)
        {
            var result = new Result<UserQueue>();
            if (userId < 1)
            {
                throw new ArgumentException("invalid userId");
            }
            var id = _identityFactory.NextId();

            var userQueue = new UserQueue
            {
                Id = id,
                QueueName = GenerateEventsQueueName(id),
                UserId = userId
            };

            await _userQueueRepository.Create(userQueue);
            // TODO: need to think about the logic to kill this queue.
            _rabbitMqClient.DeclareQueue(userQueue.QueueName);

            result.SuccessResult = userQueue;
            return result;
        }


        public async Task<Result<ClientEvent>> GetEvents(long userId, string queueName)
        {
            var result = new Result<ClientEvent>();

            // TODO: this info should be retrieved from some cache or some fast retrieval storage rather than in DB.
            // DB should just be a fallback.
            var userQueue = await _userQueueRepository.Get(userId, queueName);

            if (userQueue == null || userQueue.Id < 1)
            {
                result.ErrorMessages.Add($"No queue found with name {queueName} for user: {userId}");
                return result;
            }

            await FetchEventsUntilTimeout(queueName, result);
            return result;
        }

        private async Task FetchEventsUntilTimeout(string queueName, Result<ClientEvent> result)
        {

            var taskCompletionSource = new TaskCompletionSource<string>();
            var messagesTask = taskCompletionSource.Task;

            using (var channel = _rabbitMqClient.GetChannel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($"message: {message}");
                    taskCompletionSource.SetResult(message);
                    //TODO: test this changes.
                    return Task.CompletedTask;
                };
                // TODO: make consumption of the events more robust.
                channel.BasicConsume(queueName, true, consumer: consumer);

                var timeOutTask = Task.Delay(_eventOptions?.Timeout ?? 10000);

                await Task.WhenAny(messagesTask, timeOutTask);


                if (messagesTask.IsCompletedSuccessfully)
                {
                    result.SuccessResult = JsonConvert.DeserializeObject<ClientEvent>(messagesTask.Result);
                }
            }

        }
        private static string GenerateEventsQueueName(long userQueueId) => $"userevents.{userQueueId}";

        // Publishes events to clients
        public async Task PublishEvent(PublishEventRequest publishEventRequest)
        {
            if (publishEventRequest is null)
            {
                throw new ArgumentNullException(nameof(publishEventRequest));
            }

            var userQueues = await _userQueueRepository.GetAllUserQueues(publishEventRequest.UserId);
            if (userQueues != null && userQueues.Any())
            {
                _eventsPublisher.PublishEvents(publishEventRequest, userQueues);
            }
        }

        public void CreateClientEvent(PublishEventRequest publishEventRequest)
        {
            if (publishEventRequest is null)
            {
                throw new ArgumentNullException(nameof(publishEventRequest));
            }

            _rabbitMqClient.PushToExchange(Utils.Common.Constants.EventsExchangeName, publishEventRequest);
        }
    }
}