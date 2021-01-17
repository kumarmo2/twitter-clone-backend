using RabbitMQ.Client;

namespace Utils.Common
{
    public interface IRabbitMqClient
    {
        void DeclareQueue(string queueName);
        IModel GetChannel();
    }
}