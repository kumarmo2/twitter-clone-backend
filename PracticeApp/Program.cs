using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using CommonLibs.RedisCache;
using System.Text.Json;

namespace PracticeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                .Build();

            CachePractice(services, config);
        }

        private static void CachePractice(IServiceCollection services, IConfiguration config)
        {
            services.AddRedisCacheManager(config);
            var serviceProvider = services.BuildServiceProvider();

            var cacheMananger = serviceProvider.GetService<IRedisCacheManager>();
            // >>>>>>>> Get Value <<<<<<<<<<<<<

            // var value = cacheMananger.GetDatabase().StringGet("name");
            // if (value.IsNull)
            // {
            //     Console.WriteLine("value is null");
            // }
            // else
            // {
            //     Console.WriteLine($"Value: {value.ToString()}");
            // }

            // >>>>>>>> Get Value <<<<<<<<<<<<<

            // Set Integer
            // cacheMananger.GetDatabase().StringSet("intVal", JsonSerializer.Serialize<int>(1));

            // var value = cacheMananger.GetDatabase().StringGet("intVal");

            // if (value.IsNull)
            // {
            //     Console.WriteLine("value is null");
            // }
            // else
            // {
            //     Console.WriteLine($"Value: {JsonSerializer.Deserialize<int>(value.ToString())}");
            // }
            // cacheMananger.SetRecord<int>("intVal", 123);
            Console.WriteLine($"value: {cacheMananger.GetRecord<int>("intVal").Result}");

        }
    }
}
