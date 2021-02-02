using System.Threading.Tasks;
using Models.Tweets;
using Models.Users;

namespace DataAccess.Users
{
    public interface IFollowRepository
    {
        Task<Follow> GetFollow(long followerId, long followeeId);
        Task<Follow> GetFollowById(long followId);
        Task Create(Follow follow);
        Task Delete(long followId);
        Task UpdateStatus(long id, FollowStatus status);
    }
}