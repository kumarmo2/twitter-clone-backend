- if running envoy gives error something related to `shared memory` , run this `sudo rm /dev/shm/envoy_shared_memory_0`.
- in case some permission issues comes up, just allow read/write/exec permission to all users to the project folder and its content.

### Rate Limit

- config structure:
  - method: GET/POST/PUT/DELETE
  - path: /api/tweet
  - perMin: 3
  - perDay: 500

### NewsFeedTimeLine Algo (Testing Pending)

- When a Tweet Is Created

  - Find all the friends of the tweet's author.
    - Add the tweetId in the `newsfeedtimeline` of the friend.
  - Also add the tweet in the `newsfeedtimeline` of the author.

- When a tweet is deleted
  - Find all the friends of the tweet's author.
    - delete the tweetId from the `newsfeedtimeline` of the friend.
  - Also delete the tweet from the `newsfeedtimeline` of the author.

### Design TODOS.

- Currently, the timeline(Home and NewsFeed) data, is saved in Redis. And even though we only save the tweedIds in the Redis List  
  those list could get potentially very big. I think we should archive timeline entries, which are older than 1-2 year old. Users  
  can ask for them when needed i.e We can let them download the timeline data in some format.
