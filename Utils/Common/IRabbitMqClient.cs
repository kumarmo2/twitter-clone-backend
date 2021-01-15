namespace Utils.Common
{
    public interface IRabbitMqClient
    {
        public void DeclareQueue(string queueName);
    }
}