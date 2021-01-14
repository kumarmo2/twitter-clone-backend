using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterWeb.Business.Users;
using TwitterWeb.Dtos.Users;
using userModels = TwitterWeb.Models.Users;
using userDtos = TwitterWeb.Dtos.Users;
using TwitterWeb.Utils.Common;
using TwitterWeb.Utils.Users;
using Microsoft.AspNetCore.Http;
using TwitterWeb.Filters;

namespace TwitterWeb.Controllers.Users
{
    public class UsersController : CommonApiController
    {
        private readonly IUsersLogic _usersLogic;
        private readonly IUserUtils _userUtils;
        public UsersController(IUsersLogic usersLogic, IUserUtils userUtils)
        {
            _usersLogic = usersLogic;
            _userUtils = userUtils;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(CreateUserRequest createUserRequest)
        {

            // TODO: check what happens when wrong input is given. 
            var result = await _usersLogic.CreateUser(createUserRequest);

            if (result.SuccessResult == null || result.SuccessResult.Id < 1)
            {
                return Ok(result);
            }
            var user = result.SuccessResult;
            AddAuthCookie(Response, user.Id);

            return Ok(GetUserDto(user));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginRequest loginRequest)
        {
            var result = await _usersLogic.LoginUser(loginRequest);

            if (result.SuccessResult == null || result.SuccessResult.Id < 1)
            {
                return Ok(result);
            }
            AddAuthCookie(Response, result.SuccessResult.Id);
            return Ok(GetUserDto(result.SuccessResult));
        }

        [ServiceFilter(typeof(Authorization))]
        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            Response.Cookies.Delete(Constants.AuthCookieName);
            return Ok();
        }

        private void AddAuthCookie(HttpResponse response, long userId)
        {
            var token = _userUtils.GenerateUserAuthJwt(userId);
            Response.Cookies.Append(Constants.AuthCookieName, token);
        }

        private static userDtos.User GetUserDto(userModels.User user) =>
          new userDtos.User
          {
              DisplayName = user.DisplayName,
              Email = user.Email,
              FirstName = user.FirstName,
              Handle = user.Handle,
              Id = user.Id,
              LastName = user.LastName
          };
    }
}