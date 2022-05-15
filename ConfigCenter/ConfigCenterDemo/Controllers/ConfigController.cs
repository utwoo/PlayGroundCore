using System.Threading.Tasks;
using ConfigCenterDemo.Extensions;
using ConfigCenterDemo.Models;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConfigCenterDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ILogger<ConfigController> _logger;
        private readonly IConsulClient _consulClient;
        private readonly IConfiguration _configuration;

        public ConfigController(
            ILogger<ConfigController> logger,
            IConsulClient consulClient,
            IConfiguration configuration)
        {
            _logger = logger;
            _consulClient = consulClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] string key)
        {
            var settingModel = await _consulClient.GetValueAsync<SettingModel>(key);
            return settingModel.Message;
        }

        [HttpGet("{key}")]
        public IActionResult GetValueForKey(string key)
        {
            return Ok(_configuration[key]);
        }
    }
}