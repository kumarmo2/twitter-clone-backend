using System.Linq;
using System.Threading.Tasks;
using Business.Events;
using Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Utils.Common;

namespace TwitterWeb.Controllers.Events
{
    [Route("/api/events/register")]
    [ServiceFilter(typeof(Authorization))]
    public class RegisterController : CommonApiController
    {
        private readonly IEventsLogic _eventsLogic;
        public RegisterController(IEventsLogic eventsLogic)
        {
            _eventsLogic = eventsLogic;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUserEvents()
        {
            var userAuthDto = Request.HttpContext.Items[Constants.AuthenticatedUserKey] as UserAuthDto;

            var userQueueResult = await _eventsLogic.RegisterUserEvent(userAuthDto.UserId);
            if (userQueueResult == null)
            {
                return StatusCode(500);
            }
            if (userQueueResult.ErrorMessages.Any())
            {
                return Ok(userQueueResult);
            }

            return Ok(new
            {
                queueName = userQueueResult.SuccessResult.QueueName
            });
        }
    }
}