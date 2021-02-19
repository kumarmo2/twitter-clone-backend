using System;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace Utils.Common
{
    // TODO: Should be moved to CommonLibs.
    public class RabbitMqClient : IRabbitMqClient
    {
        private static IConnection _connection;
        static RabbitMqClient()
        {
            var cf = new ConnectionFactory { DispatchConsumersAsync = true };
            cf.Uri = new Uri("amqp://guest:guest@localhost:5672/");
            _connection = cf.CreateConnection();
            Console.WriteLine("================================================ In static constructor===============================");
        }

        public void DeclareQueue(string queueName)
        {
            using (var channel = _connection.CreateModel())
            {
                // TODO: we should make an Connection/Object Pool of channels instead of always creating new ones.
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
            }
        }

        public IModel GetChannel() => _connection.CreateModel();

        public void PushToExchange<T>(string exchangeName, T payload, string exchangeType = "fanout", string routingKey = "")
        where T : class
        {
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, "fanout", durable: true, autoDelete: false, arguments: null);
                var json = JsonConvert.SerializeObject(payload);
                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: null, body: Encoding.UTF8.GetBytes(json));
            }
        }

        public void PushToQueue<T>(string queueName, T payload)
        where T : class
        {
            using (var channel = _connection.CreateModel())
            {
                // TODO: currently we can have queues for which no user registered as we don't have a logic for
                // deleting the inactive queues. That is why we still can have queues mapping in the db but not actual
                // queue created in RabbitMq. That is why we are declaring the queue first so that no exception is thrown.

                // TODO: need to add the logic for deleting the queues on priority.
                var json = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);
                channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: null, mandatory: false, body: json);
            }
        }
    }
}