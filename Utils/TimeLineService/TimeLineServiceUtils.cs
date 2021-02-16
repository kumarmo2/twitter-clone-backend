namespace Utils.TimeLineService
{
    public class TimeLineServiceUtils : ITimeLineServiceUtils
    {
        public string GetHomeTimeLineCacheKey(long userId) => $"user:{userId}:hometimeline";
    }
}