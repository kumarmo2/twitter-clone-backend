using System.Threading.Tasks;
using Dtos;
using Models.Events;

namespace Business.Events
{
    public interface IEventsLogic
    {
        Task<Result<UserQueue>> RegisterUserEvent(long userId);
        Task<Result<string>> GetEvents(long userId, string queueName);
    }
}