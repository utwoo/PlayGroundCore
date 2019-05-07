using System;
using Refit;
using RefitDemo.GithubApi.Models;

namespace RefitDemo.Interfaces.GithubApi
{
    [Headers("User-Agent: Refit Demo")]
    public interface IGithubApi
    {
        [Get("/users/{name}")]
        IObservable<User> GetUserAsync(string name); 
    }
}
