namespace Utils.Common
{
    public interface IJsonUtils
    {
        string SerializeToJson<T>(T payload);
    }
}