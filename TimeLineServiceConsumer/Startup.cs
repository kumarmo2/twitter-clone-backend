using Microsoft.Extensions.DependencyInjection;
using TimeLineServiceConsumer.Controllers;
using CommonLibs.ConsumerServer.Abstractions;
using CommonLibs.RedisCache;
using Utils.Common;
using Utils;
using DataAccess.Tweets;
using DataAccess;
using Business.TimeLineService;
using Microsoft.Extensions.Configuration;
using System.IO;
using DataAccess.TimeLineService;
using Utils.TimeLineService;
using Business.Users;
using DataAccess.Users;

namespace TimeLineServiceConsumer
{
    public class Startup
    {

        private readonly IConfiguration _config;
        public Startup()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                .Build();
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TimeLineServiceController>();
            services.AddSingleton<HomeTimeLineService>();
            services.AddSingleton<NewsFeedTimeLineService>();
            services.AddSingleton<ITimeLineServiceFactory, TimeLineServiceFactory>();
            services.AddSingleton<IFollowsLogic, FollowsLogic>();
            services.AddSingleton<IUsersLogic, UsersLogic>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ITimeLineRepository, TimeLineRepository>();
            services.AddSingleton<ITimeLineServiceUtils, TimeLineServiceUtils>();
            services.AddSingleton<ITimeLineServiceLogic, TimeLineServiceLogic>();
            services.AddSingleton<IConsumerServer, TimeLineServiceConsumerServer>();
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<IIdentityFactory, IdentityFactory>();
            services.AddSingleton<ITweetRepository, TweetRepository>();
            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<IJsonUtils, JsonUtils>();

            services.AddRedisCacheManager(_config);
            return services;
        }
    }
}