using System;
using System.Threading.Tasks;
using DataAccess.Notifications;
using Dtos;
using Dtos.Notifications;
using Models.Notifications;
using Utils;

namespace Business.Notifications
{
    public class NotificationLogic : INotificationsLogic
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IIdentityFactory _identityFactory;
        public NotificationLogic(INotificationRepository notificationRepository, IIdentityFactory identityFactory)
        {
            _notificationRepository = notificationRepository;
            _identityFactory = identityFactory;
        }
        public async Task<GenericResult<Notification, string>> Create(CreateNotificationRequest createNotificationRequest)
        {
            if (createNotificationRequest == null)
            {
                throw new ArgumentNullException("createNotificationRequest");
            }

            var result = new GenericResult<Notification, string>();
            var notification = GetNotificationFromCreateNotificationRequest(createNotificationRequest);

            await _notificationRepository.Create(notification);

            // TODO: Generate a notification event, so the EventsService can deliver the notification to the user.


            result.SuccessResult = notification;
            return result;
        }

        private Notification GetNotificationFromCreateNotificationRequest(CreateNotificationRequest createNotificationRequest) => new Notification
        {
            Id = _identityFactory.NextId(),
            Content = createNotificationRequest.Content,
            Type = createNotificationRequest.Type,
            Url = createNotificationRequest.Url,
            UserId = createNotificationRequest.UserId
        };
    }
}