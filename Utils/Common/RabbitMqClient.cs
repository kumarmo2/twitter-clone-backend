using System;
using RabbitMQ.Client;

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
                channel.QueueDeclare(queue: queueName, durable: false);
            }
        }
    }
}