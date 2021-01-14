using Dtos.Users;

namespace Utils.Users
{
    public interface IUserUtils
    {
        string GenerateUserAuthJwt(long userId);
        bool TryValidatedAuthCookie(string authCookie, out UserAuthDto userAuthDto);
    }
}