namespace Utils.TimeLineService
{
    public interface ITimeLineServiceUtils
    {
        string GetHomeTimeLineCacheKey(long userId);
        string GetNewsFeedTimeLineCacheKey(long userId);
    }
}