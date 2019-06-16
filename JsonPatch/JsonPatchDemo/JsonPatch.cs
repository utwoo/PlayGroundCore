using JsonPatchDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace JsonPatchDemo
{
    public static class JsonPatch
    {
        [FunctionName("RequestPersonPatch")]
        public static async Task<IActionResult> RequestPersonPatch(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET")] HttpRequest req,
        ILogger logger,
        ExecutionContext executionContext)
        {
            // create json patch
            JsonPatchDocument<Person> jsonPatch = new JsonPatchDocument<Person>();
            jsonPatch.Replace(prop => prop.Age, 36);
            jsonPatch.Add(prop => prop.Name, "Utwoo");

            // serialize
            var serializedItemToUpdate = JsonConvert.SerializeObject(jsonPatch);

            // create the patch request
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, "/api/PersonPatch")
            {
                Content = new ObjectContent<JsonPatchDocument<Person>>(jsonPatch, new JsonMediaTypeFormatter())
            };

            // send it, using an HttpClient instance
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:7071/");
            var response = await client.SendAsync(request);

            var result = response.Content.ReadAsStringAsync();

            return new OkObjectResult(result);
        }

        [FunctionName("PersonPatch")]
        public static async Task<IActionResult> PersonPatch(
            [HttpTrigger(AuthorizationLevel.Anonymous, "PATCH")]HttpRequest request,
            ILogger logger,
            ExecutionContext executionContext)
        {
            // get patch info
            var content = await request.ReadAsStringAsync();
            JsonPatchDocument<Person> personPatch = JsonConvert.DeserializeObject<JsonPatchDocument<Person>>(content);

            // apply patch to object.
            Person orgPerson = new Person { Age = 37, Name = "ZhuXiang" };
            personPatch.ApplyTo(orgPerson);

            return new OkObjectResult(orgPerson);
        }
    }
}
