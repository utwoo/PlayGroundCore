using System;
using Nest;

namespace NESTDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = "http://192.168.227.131:9200";
            var node = new Uri(server);
            var settings = new ConnectionSettings(node);
            var client = new ElasticClient(settings);

            var tweet = new Tweet
            {
                Id = 1,
                User = "kimchy",
                PostDate = DateTime.Now,
                Message = "Trying out Nest"
            };

            var response = client.Index(tweet, idx => idx.Index("mytweetindex"));

            var result = client.Get<Tweet>(1, idx => idx.Index("mytweetindex")).Source;

            Console.WriteLine("Success!");
            Console.ReadKey();
        }
    }
}
