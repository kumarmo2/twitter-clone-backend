using Models.Tweets;
using Dapper;
using System.Threading.Tasks;

namespace DataAccess.Tweets
{
    public class TweetRepository : ITweetRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public TweetRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        public async Task Create(Tweet tweet)
        {
            var query = @"insert into tweets.tweets(id, authorid, content, parenttweetid, quotedtweetid, retweetedtweetid)
                          values
                          (@id, @authorid, @content, @parenttweetid, @quotedtweetid, @retweetedtweetid)
                        ";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                await con.ExecuteAsync(query,
                    new
                    {
                        id = tweet.Id,
                        authorid = tweet.AuthorId,
                        content = tweet.Content,
                        parenttweetid = tweet.ParentTweetId,
                        quotedtweetid = tweet.QuotedTweetId,
                        retweetedtweetid = tweet.RetweetedTweetId
                    });
            }

        }

        public async Task<Tweet> Get(long id)
        {
            var query = "select * from tweets.tweets where id = id";
            using (var con = _dbConnectionFactory.GetDbConnection())
            {
                return await con.QueryFirstOrDefaultAsync<Tweet>(query, new { id = id });
            }
        }
    }
}