using System.IO;
using Business.Events;
using DataAccess;
using DataAccess.Events;
using Dtos.Events;
using EventsConsumer.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utils;
using Utils.Common;

namespace EventsConsumer
{
    public class StartUp
    {
        private IConfiguration _config { get; set; }
        public StartUp()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                .Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            if (services is null)
            {
                throw new System.ArgumentNullException(nameof(services));
            }

            services.AddSingleton<EventsController>();
            services.AddSingleton<IServer, Server>();
            services.AddSingleton<IEventsLogic, EventsLogic>();
            services.AddSingleton<IIdentityFactory, IdentityFactory>();
            services.AddSingleton<IUserQueueRepository, UserQueueRepository>();
            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<IEventsPublisher, EventsPublisher>();
            services.Configure<EventOptions>(_config.GetSection(EventOptions.Key));
        }
    }
}