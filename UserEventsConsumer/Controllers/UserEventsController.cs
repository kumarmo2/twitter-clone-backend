using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Users;
using Dtos.Users;

namespace UserEventsConsumer.Controllers
{
    public class UserEventsController
    {
        private readonly IUserEventsLogic _userEventsLogic;
        public UserEventsController(IUserEventsLogic userEventsLogic)
        {
            _userEventsLogic = userEventsLogic;
        }
        public async Task ProcessEvent(UserEvent userEvent)
        {
            if (userEvent == null)
            {
                throw new ArgumentNullException("userEvent");
            }
            if (userEvent.FollowId < 1)
            {
                throw new ArgumentException("Invalid FollowId");
            }
            var result = await _userEventsLogic.ProcessUserEvent(userEvent);
            if (!result.SuccessResult)
            {
                Console.WriteLine($">>> Error processing the user event, error: {result.Error}");
                // TODO: check what is the error and requeue if possible.
            }

        }

        public Task ProcessEvent(string userEvent)
        {
            Console.WriteLine($"Processing event: {Thread.CurrentThread.Name}");
            return Task.CompletedTask;
        }
    }
}