using Models.Tweets;

namespace DataAccess.Tweets
{
    public interface ILikeRepository
    {
        void Create(Like like);
        void Delete(long likeId);
    }
}