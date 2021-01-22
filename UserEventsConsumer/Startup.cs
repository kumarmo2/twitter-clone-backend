using Business.Users;
using DataAccess;
using DataAccess.Users;
using Microsoft.Extensions.DependencyInjection;
using UserEventsConsumer.Controllers;
using Utils.Common;

namespace UserEventsConsumer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<UserEventsController>();
            services.AddSingleton<IServer, Server>();
            services.AddSingleton<IUserEventsLogic, UserEventsLogic>();
            services.AddSingleton<IFollowRepository, FollowRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
        }
    }
}