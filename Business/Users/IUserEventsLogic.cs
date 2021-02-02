
using System.Threading.Tasks;
using Dtos;
using Dtos.Users;

namespace Business.Users
{
    public interface IUserEventsLogic
    {
        Task<GenericResult<bool, string>> ProcessUserEvent(UserEvent userEvent);
    }
}