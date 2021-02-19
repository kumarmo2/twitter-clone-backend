using System.Collections.Generic;
using System.Threading.Tasks;
using Dtos.TimeLineService;

namespace DataAccess.TimeLineService
{
    public interface ITimeLineRepository
    {
        Task<long> AddToHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry);
        Task<long> DeleteTweetFromHomeTimeLine(long userId, HomeTimeLineEntry homeTimeLineEntry);
    }
}