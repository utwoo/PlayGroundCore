using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtBearerDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        [Authorize(Roles = "system")]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpPost]
        public IActionResult Authenticate()
        {
            var jwtConfig = _configuration.GetSection("Jwt");
            var securityKey =
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.GetValue<string>("Secret"))),
                    SecurityAlgorithms.HmacSha256);
            //Claim，JwtRegisteredClaimNames中预定义了好多种默认的参数名，也可以像下面的Guid一样自己定义键名.
            //ClaimTypes也预定义了好多类型如role、email、name。Role用于赋予权限，不同的角色可以访问不同的接口
            //相当于有效载荷
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, jwtConfig.GetValue<string>("Iss")),
                new Claim(JwtRegisteredClaimNames.Aud, jwtConfig.GetValue<string>("Aud")),
                new Claim("Guid", Guid.NewGuid().ToString("D")),
                new Claim(ClaimTypes.Role, "admin")
            };
            SecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: securityKey,
                expires: DateTime.Now.AddMinutes(2), //过期时间
                claims: claims
            );
            //生成jwt令牌
            return Content(new JwtSecurityTokenHandler().WriteToken(securityToken));
        }
    }
}