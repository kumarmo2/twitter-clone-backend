using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos;
using Models.Tweets;

namespace Business.Users
{
    public interface IFollowsLogic
    {
        Task<Result<bool>> CreateFollow(FollowRequest createFollowRequest);
        Task<Result<bool>> AcceptFollowRequest(FollowRequest followRequest);
        Task<GenericResult<Follow, string>> GetFollow(long followId);
        Task<Result<List<Follow>>> GetFollowers(long userId);
    }
}