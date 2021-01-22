using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;

namespace Utils.Common
{
    public class RabbitMqClient : IRabbitMqClient
    {
        private static IConnection _connection;
        static RabbitMqClient()
        {
            var cf = new ConnectionFactory();
            cf.Uri = new Uri("amqp://guest:guest@localhost:/");
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
                channel.ExchangeDeclare(exchangeName, "fanout", durable: false, autoDelete: false, arguments: null);
                var json = JsonConvert.SerializeObject(payload);
                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: null, body: Encoding.UTF8.GetBytes(json));
            }
        }
    }
}