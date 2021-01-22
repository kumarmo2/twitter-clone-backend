using System;
using System.Threading.Tasks;
using DataAccess.Users;
using Dtos;
using Dtos.Users;

namespace Business.Users
{
    public class UserEventsLogic : IUserEventsLogic
    {
        private readonly IFollowsLogic _followsLogic;
        private readonly IUserRepository _userRepository;
        public UserEventsLogic(IFollowsLogic followsLogic, IUserRepository userRepository)
        {
            _followsLogic = followsLogic;
            _userRepository = userRepository;
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

            // TODO: create Notification.


            return result;
        }
    }
}