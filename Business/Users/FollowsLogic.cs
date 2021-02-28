using System.Threading.Tasks;
using Dtos;
using System;
using DataAccess.Users;
using Models.Users;
using Models.Tweets;
using Utils;
using Dtos.Users;
using System.Collections.Generic;
using System.Linq;

namespace Business.Users
{
    public class FollowsLogic : IFollowsLogic
    {
        private readonly IFollowRepository _followRepository;
        private readonly IIdentityFactory _identityFactory;
        private readonly IUserEventsPublisher _userEventsPublisher;
        public FollowsLogic(IFollowRepository followRepository, IIdentityFactory identityFactory,
        IUserEventsPublisher userEventsPublisher)
        {
            _followRepository = followRepository;
            _identityFactory = identityFactory;
            _userEventsPublisher = userEventsPublisher;
        }

        public async Task<Result<bool>> AcceptFollowRequest(FollowRequest followRequest)
        {
            if (followRequest == null)
            {
                throw new ArgumentNullException("createFollowRequest");
            }
            var result = new Result<bool>();

            if (followRequest.FolloweeId < 1)
            {
                result.ErrorMessages.Add("FolloweeId cannot be less than 1");
            }
            if (followRequest.FollowerId < 1)
            {
                result.ErrorMessages.Add("FollowerId cannot be less than 1");
            }
            if (followRequest.FollowerId == followRequest.FolloweeId)
            {
                result.ErrorMessages.Add("Kuch Bhi? -_-");
            }
            if (result.ErrorMessages.Count > 0)
            {
                return result;
            }
            var follow = await _followRepository.GetFollow(followRequest.FollowerId, followRequest.FolloweeId);
            if (follow == null || follow.Id < 1)
            {
                result.ErrorMessages.Add("Could not find Follow request");
                return result;
            }

            if (follow.Status == FollowStatus.Accepted)
            {
                result.ErrorMessages.Add("Request Already Accepted");
                return result;
            }

            await _followRepository.UpdateStatus(follow.Id, FollowStatus.Accepted);

            var userEvent = new UserEvent
            {
                EventType = UserEventType.FollowRequestAccept,
                FollowId = follow.Id
            };
            _userEventsPublisher.Publish(userEvent);

            result.SuccessResult = true;
            return result;
        }

        public async Task<Result<bool>> CreateFollow(FollowRequest createFollowRequest)
        {
            if (createFollowRequest == null)
            {
                throw new ArgumentNullException("createFollowRequest");
            }
            var result = new Result<bool>();

            if (createFollowRequest.FolloweeId < 1)
            {
                result.ErrorMessages.Add("FolloweeId cannot be less than 1");
            }
            if (createFollowRequest.FollowerId < 1)
            {
                result.ErrorMessages.Add("FollowerId cannot be less than 1");
            }
            if (createFollowRequest.FollowerId == createFollowRequest.FolloweeId)
            {
                result.ErrorMessages.Add("Kuch Bhi? -_-");
            }
            if (result.ErrorMessages.Count > 0)
            {
                return result;
            }

            var follow = await _followRepository.GetFollow(createFollowRequest.FollowerId, createFollowRequest.FolloweeId);

            if (follow != null)
            {
                if (follow.Status == FollowStatus.Accepted)
                {
                    result.ErrorMessages.Add("Already Following");
                }
                else if (follow.Status == FollowStatus.Pending)
                {
                    result.ErrorMessages.Add("Follow Request Pending Approval");
                }
                return result;
            }

            follow = GetFollow(createFollowRequest);
            await _followRepository.Create(follow);

            var userEvent = new UserEvent
            {
                EventType = UserEventType.FollowRequestCreate,
                FollowId = follow.Id
            };

            _userEventsPublisher.Publish(userEvent);


            result.SuccessResult = true;

            return result;
        }

        public async Task<GenericResult<Follow, string>> GetFollow(long followId)
        {
            var result = new GenericResult<Follow, string>();
            if (followId < 1)
            {
                result.Error = "Invalid Follow Id";
                return result;
            }

            var follow = await _followRepository.GetFollowById(followId);
            if (follow == null || follow.Id < 1)
            {
                result.Error = $"Could not find follow with the id {followId}";
                return result;
            }
            result.SuccessResult = follow;
            return result;

        }

        public async Task<Result<List<Follow>>> GetFollowers(long userId)
        {
            var result = new Result<List<Follow>>();

            var followers = await _followRepository.GetFollowers(userId);
            if (followers == null || !followers.Any())
            {
                result.SuccessResult = new List<Follow>();
                return result;
            }
            result.SuccessResult = followers;
            return result;

        }

        private Follow GetFollow(FollowRequest createFollowRequest)
        {
            var id = _identityFactory.NextId();
            var follow = new Follow
            {
                Id = id,
                FolloweeId = createFollowRequest.FolloweeId,
                FollowerId = createFollowRequest.FollowerId,
                Status = FollowStatus.Pending
            };
            return follow;
        }
    }
}