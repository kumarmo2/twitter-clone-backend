using System;
using System.Threading.Tasks;
using Business.Notifications;
using DataAccess.Users;
using Dtos;
using Dtos.Notifications;
using userModels = Models.Users;
using Dtos.Users;
using Models.Notifications;

namespace Business.Users
{
    public class UserEventsLogic : IUserEventsLogic
    {
        private readonly IFollowsLogic _followsLogic;
        private readonly IUserRepository _userRepository;
        private readonly INotificationsLogic _notificationsLogic;
        public UserEventsLogic(IFollowsLogic followsLogic, IUserRepository userRepository,
            INotificationsLogic notificationsLogic)
        {
            _followsLogic = followsLogic;
            _userRepository = userRepository;
            _notificationsLogic = notificationsLogic;
        }
        public async Task<GenericResult<bool, string>> ProcessUserEvent(UserEvent userEvent)
        {
            if (userEvent == null)
            {
                throw new ArgumentNullException();
            }

            var result = new GenericResult<bool, string>();

            var followResult = await _followsLogic.GetFollow(userEvent.FollowId);
            if (followResult == null)
            {
                result.Error = "Something went wrong";
                result.SuccessResult = false;
                return result;
            }
            if (followResult.Error != null)
            {
                result.Error = followResult.Error;
                result.SuccessResult = true;
                return result;
            }
            var follow = followResult.SuccessResult;
            var followerTask = _userRepository.Get(follow.FollowerId);
            var followeeTask = _userRepository.Get(follow.FolloweeId);

            await Task.WhenAll(followerTask, followeeTask);

            var follower = followerTask.Result;
            var followee = followeeTask.Result;

            var notificationResult = await CreateNotification(userEvent, follower, followee);
            result.SuccessResult = true;
            return result;
        }

        private async Task<GenericResult<Notification, string>> CreateNotification(UserEvent userEvent, userModels.User follower, userModels.User followee)
        {
            // TODO: ideally these notificationtypeids should be fetched from persistent layer.
            long notificationType = userEvent.EventType == UserEventType.FollowRequestCreate ? 1 : 2;

            var content = notificationType == 1 ? $"{follower.DisplayName} has requested to follow you" :
                $"{followee.DisplayName} has accepted your follow request";

            long userId = userEvent.EventType == UserEventType.FollowRequestCreate ? followee.Id : follower.Id;

            var createNotificationRequest = new CreateNotificationRequest
            {
                UserId = userId,
                Content = content,
                Type = notificationType
            };
            return await _notificationsLogic.Create(createNotificationRequest);
        }
    }
}