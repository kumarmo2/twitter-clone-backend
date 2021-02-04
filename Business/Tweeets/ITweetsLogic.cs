using System.Threading.Tasks;
using Dtos;
using Dtos.Tweets;
using tweetModels = Models.Tweets;

namespace Business.Tweets
{
    public interface ITweetsLogic
    {
        Task<GenericResult<tweetModels.Tweet, string>> CreateTweet(CreateTweetRequest createTweetRequest);
    }
}