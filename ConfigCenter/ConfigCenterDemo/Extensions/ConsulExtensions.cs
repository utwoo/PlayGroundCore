using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Newtonsoft.Json;

namespace ConfigCenterDemo.Extensions
{
    public static class ConsulExtensions
    {
        public static async Task<T> GetValueAsync<T>(
            this IConsulClient client,
            string key)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            var getPair = await client.KV.Get(key);
            if (getPair?.Response == null)
                return default(T);

            var value = Encoding.UTF8.GetString(getPair.Response.Value);

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}