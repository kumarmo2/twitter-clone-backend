using Dtos.TimeLineService;

namespace Business.TimeLineService
{
    public interface ITimeLineServiceFactory
    {
        ITimeLineService GetTimeLineService(TimeLineType timeLineType);
    }
}