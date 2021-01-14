using TwitterWeb.Models.Users;

namespace TwitterWeb.Models.Tweets
{
  public class Follow
  {
    public long Id { get; set; }
    public long FollowerId { get; set; }
    public long FolloweeId { get; set; }
    public FollowStatus Status { get; set; }
  }
}