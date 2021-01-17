using System.Threading.Tasks;
using Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Utils.Common;
using Business.Events;

namespace EventsWeb.Controllers
{
    [Route("events")]
    [ApiController]
    [ServiceFilter(typeof(Authorization))]
    public class EventsController : ControllerBase
    {
        private readonly IEventsLogic _eventsLogic;
        public EventsController(IEventsLogic eventsLogic)
        {
            _eventsLogic = eventsLogic;
        }

        [HttpGet("{queueName}")]
        public async Task<IActionResult> GetEvents(string queueName)
        {
            var userAuthDto = Request.HttpContext.Items[Constants.AuthenticatedUserKey] as UserAuthDto;
            var userId = userAuthDto.UserId;

            var result = await _eventsLogic.GetEvents(userId, queueName);
            return Ok(result);
        }
    }
}