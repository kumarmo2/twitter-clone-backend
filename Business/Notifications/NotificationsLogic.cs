using System;
using System.Threading.Tasks;
using Business.Events;
using DataAccess.Notifications;
using Dtos;
using Dtos.Events;
using Dtos.Notifications;
using notificationDtos = Dtos.Notifications;
using Models.Notifications;
using notificationModels = Models.Notifications;
using Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Business.Notifications
{
    public class NotificationLogic : INotificationsLogic
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IIdentityFactory _identityFactory;
        private readonly IEventsLogic _eventsLogic;

        public NotificationLogic(INotificationRepository notificationRepository, IIdentityFactory identityFactory,
            IEventsLogic eventsLogic)
        {
            _notificationRepository = notificationRepository;
            _identityFactory = identityFactory;
            _eventsLogic = eventsLogic;
        }
        public async Task<GenericResult<notificationModels.Notification, string>> Create(CreateNotificationRequest createNotificationRequest)
        {
            if (createNotificationRequest == null)
            {
                throw new ArgumentNullException("createNotificationRequest");
            }

            var result = new GenericResult<notificationModels.Notification, string>();
            var notification = GetNotificationFromCreateNotificationRequest(createNotificationRequest);

            await _notificationRepository.Create(notification);

            // TODO: Generate a notification event, so the EventsService can deliver the notification to the user.
            _eventsLogic.CreateClientEvent(GetPublishEventRequestFromNotification(MapNotification(notification), createNotificationRequest.UserId));


            result.SuccessResult = notification;
            return result;
        }

        private static notificationDtos.Notification MapNotification(notificationModels.Notification notification) => new notificationDtos.Notification
        {
            Content = notification.Content,
            IsSeen = notification.IsSeen,
            Type = notification.Type,
            Url = notification.Url,
        };

        private static PublishEventRequest GetPublishEventRequestFromNotification(notificationDtos.Notification notification, long userId) =>
         new PublishEventRequest
         {
             EventData = JObject.FromObject(notification).ToObject<Dictionary<string, object>>(),
             EventType = EventType.Notification,
             UserId = userId
         };

        private notificationModels.Notification GetNotificationFromCreateNotificationRequest(CreateNotificationRequest createNotificationRequest) => new notificationModels.Notification
        {
            Id = _identityFactory.NextId(),
            Content = createNotificationRequest.Content,
            Type = createNotificationRequest.Type,
            Url = createNotificationRequest.Url,
            UserId = createNotificationRequest.UserId
        };
    }
}