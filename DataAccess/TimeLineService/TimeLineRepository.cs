using System.Threading.Tasks;
using Dtos.TimeLineService;
using CommonLibs.RedisCache;
using Utils.TimeLineService;

namespace DataAccess.TimeLineService
{
    public class TimeLineRepository : ITimeLineRepository
    {
        private readonly IRedisCacheManager _redisCacheManger;
        private readonly ITimeLineServiceUtils _timeLineServiceUtils;
        public TimeLineRepository(IRedisCacheManager redisCacheManager, ITimeLineServiceUtils timeLineServiceUtils)
        {
            _redisCacheManger = redisCacheManager;
            _timeLineServiceUtils = timeLineServiceUtils;
        }
        public async Task AddToHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry)
        {
            var cacheKey = _timeLineServiceUtils.GetHomeTimeLineCacheKey(userId);
            await _redisCacheManger.SortedSetAdd<HomeTimeLineEntry>(cacheKey, homeTimeLineEntry, homeTimeLineEntry.TweetId);
        }

        public async Task AddToNewsFeedTimeLine(long userId, NewsFeedTimeLineEntry newsFeedTimeLineEntry)
        {
            var cacheKey = _timeLineServiceUtils.GetNewsFeedTimeLineCacheKey(userId);
            await _redisCacheManger.SortedSetAdd<NewsFeedTimeLineEntry>(cacheKey, newsFeedTimeLineEntry, newsFeedTimeLineEntry.TweetId);
        }

        public async Task DeleteFromNewsFeedTimeLine(long userId, long tweetId)
        {
            var cacheKey = _timeLineServiceUtils.GetNewsFeedTimeLineCacheKey(userId);
            await _redisCacheManger.SortedSetRemoveRangeByScore(cacheKey, tweetId, tweetId);
        }

        // TODO: need to test this.
        public async Task<long> DeleteTweetFromHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry)
        {
            var cacheKey = _timeLineServiceUtils.GetHomeTimeLineCacheKey(userId);
            return await _redisCacheManger.SortedSetRemoveRangeByScore(cacheKey, homeTimeLineEntry.TweetId, homeTimeLineEntry.TweetId);
        }
    }
}