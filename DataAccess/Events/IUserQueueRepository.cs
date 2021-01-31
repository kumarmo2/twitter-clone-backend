using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Events;

namespace DataAccess.Events
{
    public interface IUserQueueRepository
    {
        Task Create(UserQueue userQueue);

        Task<UserQueue> Get(long userId, string queueName);

        Task<List<UserQueue>> GetAllUserQueues(long userId);
    }
}