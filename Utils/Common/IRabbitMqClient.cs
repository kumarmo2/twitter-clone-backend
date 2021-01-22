using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Utils.Common
{
    public interface IRabbitMqClient
    {
        void DeclareQueue(string queueName);
        IModel GetChannel();
        void PushToExchange<T>(string exchangeName, T payload, string exchangeType = "fanout", string routingKey = "")
            where T : class;
    }
}