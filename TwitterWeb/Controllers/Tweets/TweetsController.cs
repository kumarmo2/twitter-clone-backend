using System.Threading.Tasks;
using Business.Tweets;
using tweetDtos = Dtos.Tweets;
using tweetModels = Models.Tweets;
using Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Utils.Common;

namespace TwitterWeb.Controllers.Tweets
{
    public class TweetsController : CommonApiController
    {
        private readonly ITweetsLogic _tweetsLogic;
        public TweetsController(ITweetsLogic tweetsLogic)
        {
            _tweetsLogic = tweetsLogic;
        }

        [ServiceFilter(typeof(Authorization))]
        public async Task<IActionResult> CreateTweet(tweetDtos.CreateTweetRequest createTweetRequest)
        {
            var userAuthObject = Request.HttpContext.Items[Constants.AuthenticatedUserKey];
            var userAuthDto = userAuthObject as UserAuthDto;
            createTweetRequest.AuthorId = userAuthDto.UserId;

            var result = await _tweetsLogic.CreateTweet(createTweetRequest);
            if (!string.IsNullOrEmpty(result.Error))
            {
                return Ok(result);
            }
        }

        private static tweetDtos.Tweet GetTweetFromTweetModel(tweetModels.Tweet tweet) => new
        tweetDtos.Tweet
        {
            Id = tweet.Id,
            AuthorId = tweet.AuthorId,
            Content = tweet.Content
        };
    }
}