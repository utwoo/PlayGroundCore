using SwaggerDemo.Models;
using Swashbuckle.AspNetCore.Examples;

namespace SwaggerDemo.Examples
{
    public class UserExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new UserInfo
            {
                Id = 4,
                Name = "ZhuXiang",
                Age = 37,
                IsMarried = false
            };
        }
    }
}
