using Newtonsoft.Json;
namespace Utils.Common
{
    public class JsonUtils : IJsonUtils
    {
        public string SerializeToJson<T>(T payload) => JsonConvert.SerializeObject(payload);
    }
}