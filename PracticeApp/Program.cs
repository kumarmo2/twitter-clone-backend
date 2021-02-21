using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using CommonLibs.RedisCache;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

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
            var cache = cacheMananger.GetDatabase();

            // var values = cache.SortedSetRangeByScore("asdf", order: StackExchange.Redis.Order.Descending, start: 3, stop: double.NegativeInfinity, skip: 0, take: 2);
            // cache.SortedSetAdd()
            // foreach (var value in values)
            // {
            //     Console.WriteLine($"value: {value.ToString()}");
            // }
            // cache.SortedSetRemoveRangeByScore()

            // cacheMananger.GetDatabase().ListRightPush("my-list", JsonSerializer.Serialize<int>(1));
            // cacheMananger.GetDatabase().ListRightPush("my-list", JsonSerializer.Serialize<int>(2));

            // var list = cacheMananger.GetDatabase().ListRange("my-list2", 0, 10);
            // var intList = list
            //                 .Select(redisValue =>
            //                 {
            //                     return JsonSerializer.Deserialize<int>(redisValue.ToString());
            //                 }).ToList();
            // // var intList = JsonSerializer.Deserialize<List<int>>(list.ToString());
            // // Console.WriteLine($"val: {intList[1]}");
            // Console.WriteLine($"length: {intList.Count}");

            // cacheMananger.GetDatabase().ListRemoveAsync()
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
