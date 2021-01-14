using System.Threading.Tasks;
using Dtos;

namespace Business.Users
{
    public interface IFollowsLogic
    {
        Task<Result<bool>> CreateFollow(FollowRequest createFollowRequest);
        Task<Result<bool>> AcceptFollowRequest(FollowRequest followRequest);
    }
}