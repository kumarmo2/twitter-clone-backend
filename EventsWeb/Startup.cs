using Business.Events;
using DataAccess;
using DataAccess.Events;
using Dtos.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils;
using Utils.Common;
using Utils.Users;

namespace EventsWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();



            // services.Configure<EventOptions>("Events", Configuration);

            services.Configure<EventOptions>(Configuration.GetSection(EventOptions.Key));


            services.AddSingleton<IUserUtils, UserUtils>();
            services.AddSingleton<Authorization>();
            services.AddSingleton<IEventsLogic, EventsLogic>();
            services.AddSingleton<IIdentityFactory, IdentityFactory>();
            services.AddSingleton<IUserQueueRepository, UserQueueRepository>();
            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<IEventsPublisher, EventsPublisher>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
