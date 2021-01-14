using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Business.Users;
using DataAccess;
using DataAccess.Users;
using TwitterWeb.Filters;
using Utils;
using Utils.Users;
using Business.Events;
using DataAccess.Events;

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



            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUsersLogic, UsersLogic>();
            services.AddSingleton<IUserUtils, UserUtils>();
            services.AddSingleton<IFollowRepository, FollowRepository>();
            services.AddSingleton<IFollowsLogic, FollowsLogic>();
            services.AddSingleton<IEventsLogic, EventsLogic>();
            services.AddSingleton<IUserQueueRepository, UserQueueRepository>();

            services.AddSingleton<IIdentityFactory, IdentityFactory>();
            services.AddSingleton<Authorization>();
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
            app.UseStaticFiles();

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
