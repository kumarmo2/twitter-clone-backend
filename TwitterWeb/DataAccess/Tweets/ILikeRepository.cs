using TwitterWeb.Models.Tweets;

namespace TwitterWeb.DataAccess.Tweets
{
    public interface ILikeRepository
    {
        void Create(Like like);
        void Delete(long likeId);
    }
}