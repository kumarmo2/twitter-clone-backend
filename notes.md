- if running envoy gives error something related to `shared memory` , run this `sudo rm /dev/shm/envoy_shared_memory_0`.
- in case some permission issues comes up, just allow read/write/exec permission to all users to the project folder and its content.

## Rate Limit

- config structure:

  - method: GET/POST/PUT/DELETE
  - path: tweet <!-- this will be used as the endsWith path-->
  - perMin: 10
  - perSec: 3

- will support perSec and perMin only, but logic can be easily extended for further units.

- for one config will create one hash per user in redis.

  - ### Algo
    - if perMin value is available
      - get value for current sec and previous sec
      - use currSecCounter + (percentage of previous second in the window range) \* value of prevMin.
        - if this is greater than the limit, return throttle response
        - if not, we will need to check the perMin limit as well using the same above logic.
        - if all limits are passed, increment the counter of each units by 1 and let request continue on.
        - if any of the limit is crossed, return the error response with an approx time after which it can try again.

## NewsFeedTimeLine Algo (Testing Pending)

- When a Tweet Is Created

  - Find all the friends of the tweet's author.
    - Add the tweetId in the `newsfeedtimeline` of the friend.
  - Also add the tweet in the `newsfeedtimeline` of the author.

- When a tweet is deleted
  - Find all the friends of the tweet's author.
    - delete the tweetId from the `newsfeedtimeline` of the friend.
  - Also delete the tweet from the `newsfeedtimeline` of the author.

## Design TODOS.

- Currently, the timeline(Home and NewsFeed) data, is saved in Redis. And even though we only save the tweedIds in the Redis List  
  those list could get potentially very big. I think we should archive timeline entries, which are older than 1-2 year old. Users  
  can ask for them when needed i.e We can let them download the timeline data in some format.
