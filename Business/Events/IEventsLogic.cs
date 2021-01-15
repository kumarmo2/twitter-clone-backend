using System.Threading.Tasks;
using Dtos;
using Models.Events;

namespace Business.Events
{
    public interface IEventsLogic
    {
        Task<Result<UserQueue>> RegisterUserEvent(long userId);
    }
}