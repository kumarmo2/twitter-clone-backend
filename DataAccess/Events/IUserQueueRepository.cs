using System.Threading.Tasks;
using Models.Events;

namespace DataAccess.Events
{
    public interface IUserQueueRepository
    {
        Task Create(UserQueue userQueue);
    }
}