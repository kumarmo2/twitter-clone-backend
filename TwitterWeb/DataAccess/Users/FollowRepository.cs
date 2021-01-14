using System.Threading.Tasks;
using TwitterWeb.Models.Tweets;
using Dapper;
using TwitterWeb.Models.Users;
using DataAccess;

namespace TwitterWeb.DataAccess.Users
{
    public class FollowRepository : IFollowRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public FollowRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task Create(Follow follow)
        {
            var query = $@"insert into users.follows(id, followerid, followeeid, status)
                  values
                  (@id, @followerid, @followeeid, @status::users.followstatus)";

            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                await con.ExecuteAsync(query, new { id = follow.Id, followerid = follow.FollowerId, followeeid = follow.FolloweeId, status = follow.Status.ToString() });
            }
        }

        public async Task Delete(long followId)
        {
            var query = "delete from users.follows where id = @id";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                await con.ExecuteAsync(query, new { id = followId });
            }
        }

        public async Task<Follow> GetFollow(long followerId, long followeeId)
        {
            var query = "select * from users.follows where followerid = @followerid and followeeid = @followeeid";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Follow>(query, new { followerid = followerId, followeeId = followeeId });
            }
        }

        public async Task UpdateStatus(long id, FollowStatus status)
        {
            var query = @"update users.follows
                        set status = @status::users.followstatus
                        where id = @id";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                await con.ExecuteAsync(query, new { id = id, status = status.ToString() });
            }
        }
    }
}
