
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Dtos.Users;
using Utils.Users;
using System;

namespace Utils.Common
{
    public class Authorization : IAuthorizationFilter
    {
        private readonly IUserUtils _userUtils;
        public Authorization(IUserUtils userUtils)
        {
            _userUtils = userUtils;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var cookies = context.HttpContext.Request.Cookies;

            cookies.TryGetValue(Constants.AuthCookieName, out var value);

            UserAuthDto userAuthDto = null;
            if (!_userUtils.TryValidatedAuthCookie(value, out userAuthDto))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            Console.WriteLine(">>>>>>>>>>>>>authorization cleared <<<<<<<<<<<<<<<");
            context.HttpContext.Items.Add(Constants.AuthenticatedUserKey, userAuthDto);
            return;

        }
    }
}
