using Snowflake.Core;

namespace TwitterWeb.Utils
{
  public class IdentityFactory : IIdentityFactory
  {
    // TODO: Need to confirm if in a distributed environment,
    //  if each instance needs to have a different workerId?
    private static IdWorker _idWorker = new IdWorker(1, 1);
    public long NextId() =>
      _idWorker.NextId();
  }
}