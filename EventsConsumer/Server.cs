using System;
using System.Threading;
using EventsConsumer.Controllers;
using Utils.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Dtos.Events;

namespace EventsConsumer
{
    public class Server : IServer
    {
        private readonly EventsController _controller;
        private readonly IRabbitMqClient _rabbitMqClient;
        private static ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        public Server(EventsController controller, IRabbitMqClient rabbitMqClient)
        {
            _controller = controller;
            _rabbitMqClient = rabbitMqClient;
        }
        public IServer InitializeServer()
        {
            ThreadPool.SetMaxThreads(Environment.ProcessorCount, Environment.ProcessorCount);

            using (var channel = _rabbitMqClient.GetChannel())
            {
                // Create exchange if not exists.
                channel.ExchangeDeclare(exchange: Utils.Common.Constants.EventsExchangeName, "fanout", durable: true);
            }
            return this;
        }

        public void StartListeningEvents()
        {
            using (var channel = _rabbitMqClient.GetChannel())
            {
                _rabbitMqClient.DeclareQueue(Utils.Common.Constants.EventsQueueName);
                // Bind the queue.
                channel.QueueBind(Utils.Common.Constants.EventsQueueName, Utils.Common.Constants.EventsExchangeName, "", null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var request = JsonConvert.DeserializeObject<PublishEventRequest>(json);
                        await _controller.ProcessEvent(request);
                    }
                    catch (Exception ex)
                    {
                        // Do better handling of exception.
                        Console.WriteLine($"ex: {ex.ToString()}");
                    }
                };
                Console.WriteLine("================= will start listening");
                channel.BasicConsume(queue: Utils.Common.Constants.EventsQueueName, true, consumer: consumer);
                _manualResetEvent.WaitOne();
            }
        }
    }
}