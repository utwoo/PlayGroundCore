using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerDemo.Examples;
using SwaggerDemo.Models;
using Swashbuckle.AspNetCore.Examples;
using System.Collections.Generic;

namespace SwaggerDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<UserInfo> GetAllUsersAsync()
        {
            List<UserInfo> users = new List<UserInfo>
            {
                new UserInfo { Id = 1, Name = "Yooxee", Age = 35, IsMarried = false},
                new UserInfo { Id = 2, Name = "Liyi", Age = 32, IsMarried = true},
                new UserInfo { Id= 3, Name = "Lily", Age = 31, IsMarried = true}
            };

            return users;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserInfo), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserExamples))] //Response example value
        public UserInfo AddUser(UserInfo user)
        {
            return user;
        }
    }
}