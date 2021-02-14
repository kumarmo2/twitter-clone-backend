using System;
using Dtos.TimeLineService;
using Microsoft.Extensions.DependencyInjection;

namespace Business.TimeLineService
{
    public class TimeLineServiceFactory : ITimeLineServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public TimeLineServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ITimeLineService GetTimeLineService(TimeLineType timeLineType)
        {
            switch (timeLineType)
            {
                case TimeLineType.Home:
                    {
                        return _serviceProvider.GetService<HomeTimeLineService>();
                    }
                default:
                    {
                        throw new Exception("Does not implement ITimeLineService for default case");
                    }
            }
        }
    }
}