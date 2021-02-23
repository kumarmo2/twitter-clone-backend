using System.Threading.Tasks;
using Dtos.TimeLineService;

namespace DataAccess.TimeLineService
{
    public interface ITimeLineRepository
    {
        Task AddToHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry);
        Task<long> DeleteTweetFromHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry);
        Task AddToNewsFeedTimeLine(long userId, NewsFeedTimeLineEntry newsFeedTimeLineEntry);
    }
}