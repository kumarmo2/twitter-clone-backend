using System;
using System.Threading.Tasks;
using DataAccess.Events;
using Dtos;
using Models.Events;
using Utils;
using Utils.Common;

namespace Business.Events
{
    public class EventsLogic : IEventsLogic
    {
        private readonly IIdentityFactory _identityFactory;
        private readonly IUserQueueRepository _userQueueRepository;
        private readonly IRabbitMqClient _rabbitMqClient;
        public EventsLogic(IIdentityFactory identityFactory, IUserQueueRepository userQueueRepository,
            IRabbitMqClient rabbitMqClient)
        {
            _identityFactory = identityFactory;
            _userQueueRepository = userQueueRepository;
            _rabbitMqClient = rabbitMqClient;
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

        private static string GenerateEventsQueueName(long userQueueId) => $"userevents.{userQueueId}";
    }
}