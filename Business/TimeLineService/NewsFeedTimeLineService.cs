using System;
using System.Threading.Tasks;
using Business.Users;
using DataAccess.Tweets;
using Dtos;
using Dtos.Tweets;
using CommonLibs.Extensions;
using System.Collections.Generic;
using System.Linq;
using DataAccess.TimeLineService;
using Dtos.TimeLineService;

namespace Business.TimeLineService
{
    public class NewsFeedTimeLineService : ITimeLineService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IFollowsLogic _followsLogic;
        private readonly IUsersLogic _usersLogic;
        private readonly ITimeLineRepository _timeLineRepository;
        public NewsFeedTimeLineService(ITweetRepository tweetRepository, IFollowsLogic followsLogic, IUsersLogic usersLogic,
         ITimeLineRepository timeLineRepository)
        {
            _tweetRepository = tweetRepository;
            _followsLogic = followsLogic;
            _usersLogic = usersLogic;
            _timeLineRepository = timeLineRepository;
        }
        // TODO: need to test the NewsFeedTimeLine functionality
        public async Task<Result<bool>> ProcessTweetEvent(TweetEvent tweetEvent)
        {
            if (tweetEvent is null)
            {
                throw new System.ArgumentNullException(nameof(tweetEvent));
            }
            var result = new Result<bool>();
            var tweet = await _tweetRepository.Get(tweetEvent.TweetId);

            if (tweet == null || tweet.Id < 1)
            {
                Console.WriteLine("Could not find any tweet");
                result.SuccessResult = true;
                return result;
            }

            var userId = tweet.AuthorId;

            var followersTask = _followsLogic.GetFollowers(userId);

            var followersResult = followersTask.Result;


            var userIds = new List<long>();
            userIds.Add(userId);

            if (followersResult.SuccessResult.IsNotEmpty())
            {
                userIds.AddRange(followersResult.SuccessResult.Select(follows => follows.FollowerId));

            }
            if (tweetEvent.Type == TweetEventType.Created)
            {
                await ProcessCreateTweetEvent(userIds, tweetEvent);
            }
            else if (tweetEvent.Type == TweetEventType.Deleted)
            {
                await ProcessDeleteTweetEvent(userIds, tweetEvent);
            }
            else
            {
                throw new BusinessException("Unhandled Tweet Event Type");
            }

            result.SuccessResult = true;

            return result;
        }

        private async Task ProcessDeleteTweetEvent(List<long> userIds, TweetEvent tweetEvent)
        {
            var tasks = userIds.Select(userId => _timeLineRepository.DeleteFromNewsFeedTimeLine(userId, tweetEvent.TweetId));
            await Task.WhenAll(tasks);
        }


        private async Task ProcessCreateTweetEvent(List<long> userIds, TweetEvent tweetEvent)
        {

            var newsFeedTimeLineEntry = new NewsFeedTimeLineEntry
            {
                TweetId = tweetEvent.TweetId
            };

            var tasks = userIds.Select(userId => _timeLineRepository.AddToNewsFeedTimeLine(userId, newsFeedTimeLineEntry));

            await Task.WhenAll(tasks);
        }
    }
}