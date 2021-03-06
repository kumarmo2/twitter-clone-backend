using System;
using System.Threading.Tasks;
using CommonLibs.RateLimiter;
using Dtos.Users;
using Microsoft.AspNetCore.Http;
using Utils.Common;
using Utils.Users;

namespace TwitterWeb.MiddleWares
{
    public class RateLimiterMiddleWare
    {
        private readonly IRateLimiter _rateLimiter;
        private readonly IUserUtils _userUtils;
        private readonly RequestDelegate _next;
        public RateLimiterMiddleWare(RequestDelegate next, IRateLimiter rateLimiter, IUserUtils userUtils)
        {
            _next = next;
            _rateLimiter = rateLimiter;
            _userUtils = userUtils;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine(">>>>>>>>>>>>>>>>>>> from class middle ware ");
            var cookies = context.Request.Cookies;

            if (cookies.TryGetValue(Constants.AuthCookieName, out var value))
            {
                Console.WriteLine($"path: {context.Request.Path}\nmethod: {context.Request.Method}");

                UserAuthDto userAuthDto = null;
                if (_userUtils.TryValidatedAuthCookie(value, out userAuthDto))
                {
                    await _rateLimiter.ShouldThrottle(context.Request.Method, context.Request.Path, userAuthDto.UserId.ToString());
                }

                // _rateLimiter.ShouldThrottle()
            }
            await _next(context);
        }
    }
}