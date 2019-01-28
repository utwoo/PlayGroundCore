using System;
using System.Threading;

namespace Redis.Alpha
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Start...");

            RedisCacheManager redis = new RedisCacheManager();
            RedisConnectionWrapper wrapper = new RedisConnectionWrapper();
            // use redlock.net to lock distribution resources.
            if (wrapper.PerformActionWithLock("TestResource", TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(1), () =>
                 {
                     Console.WriteLine("Locker Start....");
                     Thread.Sleep(10 * 1000);
                     redis.Set("test", "111", 1);
                     Console.WriteLine("Locker Complete.");
                 }))
            {
                Console.WriteLine("Successfully.");
            }
            else
            {
                Console.WriteLine("Failed.");
            }
        }
    }
}
