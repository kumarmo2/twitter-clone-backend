using System;
using CommonLibs.ConsumerServer.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace TimeLineServiceConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var serviceProvider = new Startup()
                                        .ConfigureServices(services)
                                        .BuildServiceProvider();
            serviceProvider
                .GetService<IConsumerServer>()
                .InitializeServer()
                .StartListeningEvents();

            Console.WriteLine(">>>>>> Terminating TweetsEventsConsumer <<<<<<<<<<<<<<<<");
        }
    }
}
