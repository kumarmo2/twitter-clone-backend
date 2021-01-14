using System;
using JWT.Algorithms;
using JWT.Builder;
using Dtos.Users;
using Utils.Common;

namespace Utils.Users
{
    public class UserUtils : IUserUtils
    {
        public string GenerateUserAuthJwt(long userId)
        {
            if (userId < 1)
            {
                throw new ArgumentException("invalid userid");
            }
            var algorithm = new HMACSHA256Algorithm();
            var token = new JwtBuilder()
                                .WithAlgorithm(algorithm)
                                .ExpirationTime(DateTime.Now.AddDays(30))
                                .AddClaim(Constants.JwtUserIdClaimKey, userId)
                                .WithSecret(Constants.JwtSecret)
                                .Encode();
            return token;
        }

        public bool TryValidatedAuthCookie(string authCookie, out UserAuthDto userAuthDto)
        {
            userAuthDto = null;
            if (string.IsNullOrEmpty(authCookie))
            {
                return false;
            }

            userAuthDto = new JwtBuilder()
              .WithAlgorithm(new HMACSHA256Algorithm())
              .WithSecret(Constants.JwtSecret)
              .MustVerifySignature()
              .Decode<UserAuthDto>(authCookie);

            if (userAuthDto == null || userAuthDto.UserId < 1)
            {
                return false;
            }
            return true;
        }
    }
}