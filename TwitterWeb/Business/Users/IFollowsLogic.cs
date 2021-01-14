using System.Threading.Tasks;
using TwitterWeb.Dtos;

namespace TwitterWeb.Business.Users
{
    public interface IFollowsLogic
    {
        Task<Result<bool>> CreateFollow(FollowRequest createFollowRequest);
        Task<Result<bool>> AcceptFollowRequest(FollowRequest followRequest);
    }
}