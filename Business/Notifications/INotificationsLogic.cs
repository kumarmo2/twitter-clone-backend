using System.Threading.Tasks;
using Dtos;
using Dtos.Notifications;
using Models.Notifications;

namespace Business.Notifications
{
    public interface INotificationsLogic
    {
        Task<GenericResult<Notification, string>> Create(CreateNotificationRequest createNotificationRequest);
    }
}