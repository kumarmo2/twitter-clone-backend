using System.Threading.Tasks;
using Models.Tweets;

namespace DataAccess.Tweets
{
    public interface ITweetRepository
    {
        Task Create(Tweet tweet);
        Task<Tweet> Get(long id);
    }
}