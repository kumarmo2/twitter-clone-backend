using System.Threading.Tasks;
using Dtos;
using Dtos.Tweets;

namespace Business.TimeLineService
{
    public interface ITimeLineServiceLogic
    {
        Task<Result<bool>> ProcessTweetEvent(TweetEvent tweetEvent);
    }
}