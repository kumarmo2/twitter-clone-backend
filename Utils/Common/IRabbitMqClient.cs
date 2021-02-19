using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Utils.Common
{
    // TODO: Should be moved to CommonLibs.
    public interface IRabbitMqClient
    {
        void DeclareQueue(string queueName);
        IModel GetChannel();
        void PushToExchange<T>(string exchangeName, T payload, string exchangeType = "fanout", string routingKey = "")
            where T : class;
        void PushToQueue<T>(string queueName, T payload)
            where T : class;
    }
}