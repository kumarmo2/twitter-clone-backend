using Models.Tweets;

namespace TwitterWeb.DataAccess.Tweets
{
    public interface ITweetRepository
    {
        void Create(Tweet tweet);
        void Get(long id);
    }
}