using System;
using Newtonsoft.Json;
using Refit;
using RefitDemo.Interfaces.GithubApi;

namespace RefitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var gitHubApi = RestService.For<IGithubApi>("http://api.github.com");

            gitHubApi.GetUserAsync("utwoo")
                .Subscribe(user => Console.WriteLine(JsonConvert.SerializeObject(user)));

            Console.ReadKey();
        }
    }
}
