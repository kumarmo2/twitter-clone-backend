using System.Threading.Tasks;
using Models.Notifications;
using Dapper;

namespace DataAccess.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public NotificationRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task Create(Notification notification)
        {
            var query = @"insert into notifications.notifications(id, userid, content, url, type)
                          values
                          (@id, @userid, @content, @url, @type);
                        ";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                await con.ExecuteAsync(query, new { id = notification.Id, userid = notification.UserId, content = notification.Content, url = notification.Url, type = notification.Type });
            }
        }
    }
}