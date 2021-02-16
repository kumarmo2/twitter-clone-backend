using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.TimeLineService;
using DataAccess.Tweets;
using Dtos;
using Dtos.TimeLineService;
using Dtos.Tweets;
using Utils.Common;

namespace Business.TimeLineService
{
    public class HomeTimeLineService : ITimeLineService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly ITimeLineRepository _timeLineRepository;
        private readonly IJsonUtils _jsonUtils;
        public HomeTimeLineService(ITweetRepository tweetRepository, ITimeLineRepository timeLineRepository, IJsonUtils jsonUtils)
        {
            _tweetRepository = tweetRepository;
            _timeLineRepository = timeLineRepository;
            _jsonUtils = jsonUtils;
        }
        public async Task<Result<bool>> ProcessTweetEvent(TweetEvent tweetEvent)
        {
            if (tweetEvent is null)
            {
                throw new System.ArgumentNullException(nameof(tweetEvent));
            }

            var result = new Result<bool>();

            if (tweetEvent.TweetId < 1)
            {
                result.SuccessResult = true; // This true signifies the event has been handled.
                Console.WriteLine(">>>> Invalid TweetId <<<<<<<");
                return result;
            }
            if (tweetEvent.Type == TweetEventType.Created)
            {
                result.SuccessResult = await ProcessCreateTweetEvent(tweetEvent);
                return result;
            }
            else if (tweetEvent.Type == TweetEventType.Deleted)
            {
                result.SuccessResult = await ProcessDeleteTweetEvent(tweetEvent);
                return result;
            }

            throw new BusinessException($"Unknown TweetEventType or unhandled case for the tweet: {_jsonUtils.SerializeToJson(tweetEvent)}");
        }

        private async Task<bool> ProcessDeleteTweetEvent(TweetEvent tweetEvent)
        {
            var tweet = await _tweetRepository.Get(tweetEvent.TweetId);
            if (tweet == null)
            {
                Console.WriteLine("No tweet found");
                return true;
            }
            var homeTimeLineEntry = new HomeTimeLineEntry
            {
                TweetId = tweetEvent.TweetId
            };

            var deletedItems = await _timeLineRepository.DeleteTweetFromHomeTimeLine(tweet.AuthorId, homeTimeLineEntry);
            return deletedItems > 0;
        }

        private async Task<bool> ProcessCreateTweetEvent(TweetEvent tweetEvent)
        {
            var tweet = await _tweetRepository.Get(tweetEvent.TweetId);
            if (tweet == null)
            {
                Console.WriteLine("No tweet found");
                return true;
            }

            var homeTimeLineEntry = new HomeTimeLineEntry
            {
                TweetId = tweetEvent.TweetId
            };
            await _timeLineRepository.AddToHomeTimeLine(tweet.AuthorId, homeTimeLineEntry);
            return true;
        }
    }
}