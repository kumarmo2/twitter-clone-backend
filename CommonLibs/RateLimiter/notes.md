## Components

- ConfigOptions
- ConfigProvider (one could be a local Json ConfigProvider, one could fetch config from an api etc.)
- RateLimiter

- ### ConfigProvider

  - IConfigProvider
    - JsonConfigProvider(we will use this)
    - ApiConfigprovider(this is just an example)

- ### Throttling Object Design

- The design patterns we are using:

  - Chain Of Responsibility: For processing the actual logic
  - Factory Patter: for generating the Throttler.

- ThrottleRequest

  - RateLimitConfig AppliedConfig
  - string UserId

- interface IRequestThrottler

  - bool ShouldThrottle(ThrottleRequest);

- abstract class BaseRequestThrottler: IRequestThrottler

  - private Property: nextThrottler
  - virtual method: SetNextThrottler
  - abstract method: ShouldThrottle

- Concrete Throttlers

  - class PerMinRequestThrottler: BaseRequestThrottler
  - class PerSecRequestThrottler: BaseRequestThrottler

  - NOTE: In future more types of throttle can be added

- interface IRequestThrottlerFactory
  - BaseRequestThrottle GetRequestThrottler(RateLimitConfig)
    - based on RateLimitConfig, it generates a pipeline of throttlers.
