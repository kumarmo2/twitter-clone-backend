using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace UserEventsConsumer
{
    // TODO: Eventually, this should be a part of `UsersService`.
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
