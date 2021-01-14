using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business.Users;
using Dtos.Users;
using TwitterWeb.Filters;
using Utils.Common;

namespace TwitterWeb.Controllers.Users
{
    [ServiceFilter(typeof(Authorization))]
    public class FollowsController : CommonApiController
    {
        private readonly IFollowsLogic _followLogic;
        public FollowsController(IFollowsLogic followsLogic)
        {
            _followLogic = followsLogic;
        }

        [HttpPost("{followeeId}")]
        public async Task<IActionResult> CreateFollow(long followeeId)
        {
            Request.HttpContext.Items.TryGetValue(Constants.AuthenticatedUserKey, out var userAuthDtoObject);
            var userAuthDto = userAuthDtoObject as UserAuthDto;

            var createFollowRequest = new FollowRequest
            {
                FolloweeId = followeeId,
                FollowerId = userAuthDto.UserId
            };

            var result = await _followLogic.CreateFollow(createFollowRequest);
            return Ok(result);
        }

        [HttpPost("accept/{followerId}")]
        public async Task<IActionResult> AcceptFollowRequest(long followerId)
        {
            Request.HttpContext.Items.TryGetValue(Constants.AuthenticatedUserKey, out var userAuthDtoObject);
            var userAuthDto = userAuthDtoObject as UserAuthDto;

            var followRequest = new FollowRequest
            {
                FolloweeId = userAuthDto.UserId,
                FollowerId = followerId
            };

            var result = await _followLogic.AcceptFollowRequest(followRequest);
            return Ok(result);
        }
    }
}