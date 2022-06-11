using System.Text;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;

namespace ConfigCenterDemo.Helpers
{
    public static class ConsulKeyValueProvider
    {
        public static async Task<T> GetValueAsync<T>(string key)
        {
            using var client = new ConsulClient();

            var getPair = await client.KV.Get(key);
            if (getPair?.Response == null)
                return default(T);

            var value = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}