using Dtos.Users;
using Utils.Common;

namespace DataAccess.Users
{
    public class UserEventsPublisher : IUserEventsPublisher
    {
        private readonly IRabbitMqClient _rabbitMqClient;
        public UserEventsPublisher(IRabbitMqClient rabbitMqClient)
        {
            _rabbitMqClient = rabbitMqClient;
        }
        public void Publish(UserEvent userEvent)
        {
            _rabbitMqClient.PushToExchange(Constants.UserEventsExchangeName, userEvent);
        }
    }
}