using System.Threading.Tasks;
using Models.Events;
using Dapper;

namespace DataAccess.Events
{
    public class UserQueueRepository : IUserQueueRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public UserQueueRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task Create(UserQueue userQueue)
        {
            var query = "insert into events.userqueues(id, userid, queuename) values (@id, @userid, @queuename)";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                await con.ExecuteAsync(query, new { id = userQueue.Id, userid = userQueue.UserId, queuename = userQueue.QueueName });
            }
        }

        public async Task<UserQueue> Get(long userId, string queueName)
        {
            var query = "select * from events.userqueues where userid = @userid and queuename = @queuename limit 1";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                return await con.QueryFirstOrDefaultAsync<UserQueue>(query, new { userid = userId, queuename = queueName });
            }

        }
    }
}