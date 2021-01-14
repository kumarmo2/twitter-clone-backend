using System.Threading.Tasks;
using TwitterWeb.Dtos;
using System;
using TwitterWeb.DataAccess.Users;
using TwitterWeb.Models.Users;
using TwitterWeb.Models.Tweets;
using TwitterWeb.Utils;

namespace TwitterWeb.Business.Users
{
    public class FollowsLogic : IFollowsLogic
    {
        private readonly IFollowRepository _followRepository;
        private readonly IIdentityFactory _identityFactory;
        public FollowsLogic(IFollowRepository followRepository, IIdentityFactory identityFactory)
        {
            _followRepository = followRepository;
            _identityFactory = identityFactory;
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
            result.SuccessResult = true;

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