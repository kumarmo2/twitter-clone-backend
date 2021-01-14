using System.Threading.Tasks;
using TwitterWeb.Models.Tweets;
using TwitterWeb.Models.Users;

namespace TwitterWeb.DataAccess.Users
{
    public interface IFollowRepository
    {
        Task<Follow> GetFollow(long followerId, long followeeId);
        Task Create(Follow follow);
        Task Delete(long followId);
        Task UpdateStatus(long id, FollowStatus status);
    }
}