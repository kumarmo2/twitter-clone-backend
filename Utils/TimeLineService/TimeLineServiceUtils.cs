namespace Utils.TimeLineService
{
    public class TimeLineServiceUtils : ITimeLineServiceUtils
    {
        public string GetHomeTimeLineCacheKey(long userId) => $"user:{userId}:hometimeline";

        public string GetNewsFeedTimeLineCacheKey(long userId) => $"user:{userId}:newsfeedtimeline";
    }
}