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
            return this;
        }

        public void StartListeningEvents()
        {
            using (var channel = _rabbitMqClient.GetChannel())
            {
                channel.QueueDeclare(queue: Utils.Common.Constants.UserEventsConsumerQueue, durable: false, exclusive: false, autoDelete: false);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    ThreadPool.QueueUserWorkItem<BasicDeliverEventArgs>(ConsumeMessage, ea, false);
                };

                Console.WriteLine("will start listening");
                channel.BasicConsume(queue: Utils.Common.Constants.UserEventsConsumerQueue, true, consumer: consumer);
                Console.ReadLine();

            }
        }

        private void ConsumeMessage(BasicDeliverEventArgs ea)
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                // var userEvent = JsonConvert.DeserializeObject<UserEvent>(json);
                _controller.ProcessEvent(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex: {ex.ToString()}");
            }

        }

    }
}