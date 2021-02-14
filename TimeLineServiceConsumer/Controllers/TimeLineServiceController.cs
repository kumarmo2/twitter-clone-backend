using System.Threading.Tasks;
using Business.TimeLineService;
using Business.Tweets;
using Dtos.Tweets;

namespace TimeLineServiceConsumer.Controllers
{
    public class TimeLineServiceController
    {
        private readonly ITimeLineServiceLogic _timeLineServiceLogic;
        public TimeLineServiceController(ITimeLineServiceLogic timeLineServiceLogic)
        {
            _timeLineServiceLogic = timeLineServiceLogic;
        }

        public async Task ProcessEvent(TweetEvent tweetEvent)
        {
            if (tweetEvent is null)
            {
                throw new System.ArgumentNullException(nameof(tweetEvent));
            }

            await _timeLineServiceLogic.ProcessTweetEvent(tweetEvent);
        }
    }
}