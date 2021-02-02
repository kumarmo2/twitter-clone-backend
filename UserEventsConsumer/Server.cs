using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using UserEventsConsumer.Controllers;
using Utils.Common;
using Newtonsoft.Json;
using Dtos.Users;

namespace UserEventsConsumer
{
    public class Server : IServer
    {
        private readonly UserEventsController _controller;
        private readonly IRabbitMqClient _rabbitMqClient;
        public Server(UserEventsController controller, IRabbitMqClient rabbitMqClient)
        {
            _controller = controller;
            _rabbitMqClient = rabbitMqClient;
        }

        public IServer InitializeServer()
        {
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);
            using (var channel = _rabbitMqClient.GetChannel())
            {
                // Create the exchange if not created.
                channel.ExchangeDeclare(Utils.Common.Constants.UserEventsExchangeName, "fanout", durable: true);
            }
            return this;
        }

        public void StartListeningEvents()
        {
            using (var channel = _rabbitMqClient.GetChannel())
            {
                // Create the queue if not created.
                channel.QueueDeclare(queue: Utils.Common.Constants.UserEventsConsumerQueue, durable: false, exclusive: false, autoDelete: false);

                // Bind the queue.
                channel.QueueBind(Utils.Common.Constants.UserEventsConsumerQueue, Utils.Common.Constants.UserEventsExchangeName, "", null);

                var consumer = new AsyncEventingBasicConsumer(channel); // For an async consumer, use `AsyncEventingBasicConsumer`
                channel.BasicConsume(queue: Utils.Common.Constants.UserEventsConsumerQueue, true, consumer: consumer);

                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var userEvent = JsonConvert.DeserializeObject<UserEvent>(json);
                        await _controller.ProcessEvent(userEvent);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"exception: {ex.ToString()}");
                    }
                };

                Console.WriteLine("will start listening");
                Console.ReadLine();

            }
        }

        private void ConsumeMessage(BasicDeliverEventArgs ea)
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var userEvent = JsonConvert.DeserializeObject<UserEvent>(json);
                _controller.ProcessEvent(userEvent).RunSynchronously();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex: {ex.ToString()}");
            }

        }

    }
}