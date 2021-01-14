using TwitterWeb.Dtos.Users;

namespace TwitterWeb.Utils.Users
{
  public interface IUserUtils
  {
    string GenerateUserAuthJwt(long userId);
    bool TryValidatedAuthCookie(string authCookie, out UserAuthDto userAuthDto);
  }
}