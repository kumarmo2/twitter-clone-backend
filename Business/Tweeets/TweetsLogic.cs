using System.Threading.Tasks;
using DataAccess.Tweets;
using Dtos;
using Dtos.Tweets;
using tweetModels = Models.Tweets;
using Utils;
using Utils.Common;

namespace Business.Tweets
{
    public class TweetsLogic : ITweetsLogic
    {
        private readonly IIdentityFactory _identityFactory;
        private readonly ITweetRepository _tweetRepository;
        private readonly IRabbitMqClient _rabbitMqClient;
        public TweetsLogic(IIdentityFactory identityFactory, ITweetRepository tweetRepository, IRabbitMqClient rabbitMqClient)
        {
            _identityFactory = identityFactory;
            _tweetRepository = tweetRepository;
            _rabbitMqClient = rabbitMqClient;
        }
        public async Task<GenericResult<tweetModels.Tweet, string>> CreateTweet(CreateTweetRequest createTweetRequest)
        {
            if (createTweetRequest is null)
            {
                throw new System.ArgumentNullException(nameof(createTweetRequest));
            }
            var result = new GenericResult<tweetModels.Tweet, string>();

            if (string.IsNullOrWhiteSpace(createTweetRequest.Content))
            {
                result.Error = "Content cannot be empty";
                return result;
            }
            var tweet = CreateTweetFromCreateTweetRequest(createTweetRequest);
            await _tweetRepository.Create(tweet);

            // TODO: trigger an TweetCreated Event.
            // var tweetEvent = new TweetEvent
            // {

            // }
            // _rabbitMqClient.PushToExchange(Constants.TweetsExchangeName, )

            result.SuccessResult = tweet;
            return result;
        }

        private tweetModels.Tweet CreateTweetFromCreateTweetRequest(CreateTweetRequest createTweetRequest) =>
        new tweetModels.Tweet
        {
            Id = _identityFactory.NextId(),
            AuthorId = createTweetRequest.AuthorId,
            Content = createTweetRequest.Content,
            ParentTweetId = createTweetRequest.ParentTweetId,
            QuotedTweetId = createTweetRequest.QuotedTweetId,
            RetweetedTweetId = createTweetRequest.RetweetedTweetId
        };
    }
}