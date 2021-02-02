
using System.Threading.Tasks;
using Dtos.Users;

namespace DataAccess.Users
{
    public interface IUserEventsPublisher
    {
        void Publish(UserEvent userEvent);
    }
}