using System.Threading.Tasks;
using Models.Notifications;

namespace DataAccess.Notifications
{
    public interface INotificationRepository
    {
        Task Create(Notification notification);
    }
}