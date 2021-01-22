using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace UserEventsConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            new Startup().ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var server = serviceProvider.GetService<IServer>();

            server
                .InitializeServer()
                .StartListeningEvents();
        }
    }
}
