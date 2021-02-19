using System.Threading.Tasks;
using Dtos;
using Dtos.TimeLineService;
using Dtos.Tweets;

namespace Business.TimeLineService
{
    public class TimeLineServiceLogic : ITimeLineServiceLogic
    {
        private readonly ITimeLineServiceFactory _timeLineServiceFactory;
        public TimeLineServiceLogic(ITimeLineServiceFactory timeLineServiceFactory)
        {
            _timeLineServiceFactory = timeLineServiceFactory;
        }
        public Task<Result<bool>> ProcessTweetEvent(TweetEvent tweetEvent)
        {
            if (tweetEvent is null)
            {
                throw new System.ArgumentNullException(nameof(tweetEvent));
            }

            // TODO: Need to process NewsFeedTimeLineService as well.
            return _timeLineServiceFactory.GetTimeLineService(TimeLineType.Home).ProcessTweetEvent(tweetEvent);
        }
    }
}