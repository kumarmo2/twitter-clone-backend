using System.IO;
using Business.Events;
using Business.Notifications;
using Business.Users;
using DataAccess;
using DataAccess.Events;
using DataAccess.Notifications;
using DataAccess.Users;
using Dtos.Events;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using UserEventsConsumer.Controllers;
using Utils;
using Utils.Common;

namespace UserEventsConsumer
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
            services.AddSingleton<IEventsPublisher, EventsPublisher>();
            services.AddSingleton<IEventsLogic, EventsLogic>();
            services.AddSingleton<IUserQueueRepository, UserQueueRepository>();

            services.Configure<EventOptions>(_config.GetSection(EventOptions.Key));
        }
    }
}