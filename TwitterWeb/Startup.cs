using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Business.Users;
using DataAccess;
using DataAccess.Users;
using Utils;
using Utils.Users;
using Business.Events;
using DataAccess.Events;
using Utils.Common;
using Microsoft.AspNetCore.HttpOverrides;
using DataAccess.Tweets;
using Business.Tweets;
using System;

namespace TwitterWeb
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
            services
            .AddControllersWithViews()
            // To Add NewtonSoft in dotnet 3.0 and above, you need to add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
            .AddNewtonsoftJson(options =>
            {
                options.UseCamelCasing(false);
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUsersLogic, UsersLogic>();
            services.AddSingleton<IUserUtils, UserUtils>();
            services.AddSingleton<IFollowRepository, FollowRepository>();
            services.AddSingleton<IFollowsLogic, FollowsLogic>();
            services.AddSingleton<IEventsLogic, EventsLogic>();
            services.AddSingleton<IUserQueueRepository, UserQueueRepository>();
            services.AddSingleton<IRabbitMqClient, RabbitMqClient>();
            services.AddSingleton<IUserEventsPublisher, UserEventsPublisher>();
            services.AddSingleton<IEventsPublisher, EventsPublisher>();
            services.AddSingleton<ITweetRepository, TweetRepository>();
            services.AddSingleton<ITweetsLogic, TweetsLogic>();

            services.AddSingleton<IIdentityFactory, IdentityFactory>();
            services.AddSingleton<Utils.Common.Authorization>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            }

            app.UseForwardedHeaders();
            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                Console.WriteLine(">>>>>> Request Received<<<<<<<<");
                await next.Invoke();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
