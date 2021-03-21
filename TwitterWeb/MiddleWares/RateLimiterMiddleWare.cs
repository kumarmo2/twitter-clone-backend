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

            //TODO: we need to figure out some way that we don't have to replicate the logic for authorization here.
            if (cookies.TryGetValue(Constants.AuthCookieName, out var value))
            {
                Console.WriteLine($"path: {context.Request.Path}\nmethod: {context.Request.Method}");

                UserAuthDto userAuthDto = null;
                if (_userUtils.TryValidatedAuthCookie(value, out userAuthDto))
                {
                    var shouldThrottle = await _rateLimiter.ShouldThrottle(context.Request.Method, context.Request.Path, userAuthDto.UserId.ToString());
                    if (shouldThrottle)
                    {
                        Console.WriteLine("throttling the api");
                        context.Response.StatusCode = 429;
                        return;
                    }
                }
            }
            Console.WriteLine("next middleware in the pipleine is called");
            await _next(context);
        }
    }
}