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
        public async Task<long> AddToHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry)
        {
            var cacheKey = _timeLineServiceUtils.GetHomeTimeLineCacheKey(userId);
            return await _redisCacheManger.ListLeftPush<HomeTimeLineEntry>(cacheKey, homeTimeLineEntry);
        }

        public async Task<long> DeleteTweetFromHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry)
        {
            // TODO: since hometimeline can have thousands of entries, deleting a single element
            // could take a toll on redis. as deleting an item from list is 0(n).
            var cacheKey = _timeLineServiceUtils.GetHomeTimeLineCacheKey(userId);
            return await _redisCacheManger.ListRemove(cacheKey, homeTimeLineEntry);
        }
    }
}