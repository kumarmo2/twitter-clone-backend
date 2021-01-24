using Business.Notifications;
using Business.Users;
using DataAccess;
using DataAccess.Notifications;
using DataAccess.Users;
using Microsoft.Extensions.DependencyInjection;
using UserEventsConsumer.Controllers;
using Utils;
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
            services.AddSingleton<IFollowsLogic, FollowsLogic>();
            services.AddSingleton<IIdentityFactory, IdentityFactory>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUserEventsPublisher, UserEventsPublisher>();
            services.AddSingleton<INotificationRepository, NotificationRepository>();
            services.AddSingleton<INotificationsLogic, NotificationLogic>();

        }
    }
}