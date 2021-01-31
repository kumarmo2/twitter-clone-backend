using System;
using Microsoft.Extensions.DependencyInjection;

namespace EventsConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            new StartUp()
                .ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider
                .GetService<IServer>()
                .InitializeServer()
                .StartListeningEvents();
        }
    }
}
