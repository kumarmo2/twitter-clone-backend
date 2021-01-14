using System;
using System.Threading.Tasks;
using DataAccess.Events;
using Dtos;
using Models.Events;
using Utils;

namespace Business.Events
{
    public class EventsLogic : IEventsLogic
    {
        private readonly IIdentityFactory _identityFactory;
        private readonly IUserQueueRepository _userQueueRepository;
        public EventsLogic(IIdentityFactory identityFactory, IUserQueueRepository userQueueRepository)
        {
            _identityFactory = identityFactory;
            _userQueueRepository = userQueueRepository;
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

            result.SuccessResult = userQueue;
            return result;
        }

        private static string GenerateEventsQueueName(long userQueueId) => $"userevents.{userQueueId}";
    }
}