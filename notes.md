- if running envoy gives error something related to `shared memory` , run this `sudo rm /dev/shm/envoy_shared_memory_0`.
- in case some permission issues comes up, just allow read/write/exec permission to all users to the project folder and its content.

### Design TODOS.

- Currently, the timeline(Home and NewsFeed) data, is saved in Redis. And even though we only save the tweedIds in the Redis List  
  those list could get potentially very big. I think we should archive timeline entries, which are older than 1-2 year old. Users  
  can ask for them when needed.
