using System.Threading.Tasks;
using Dtos;
using Dtos.Tweets;

namespace Business.TimeLineService
{
    public class HomeTimeLineService : ITimeLineService
    {
        public Task<Result<bool>> ProcessTweetEvent(TweetEvent tweetEvent)
        {
            throw new System.NotImplementedException();
        }
    }
}