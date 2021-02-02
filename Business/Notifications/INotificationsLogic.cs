using System.Threading.Tasks;
using Dtos;
using Dtos.Notifications;
using notificationModels = Models.Notifications;

namespace Business.Notifications
{
    public interface INotificationsLogic
    {
        Task<GenericResult<notificationModels.Notification, string>> Create(CreateNotificationRequest createNotificationRequest);
    }
}