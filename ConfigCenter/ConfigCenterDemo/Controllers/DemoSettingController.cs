using System.Threading.Tasks;
using ConfigCenterDemo.Extensions;
using ConfigCenterDemo.Models;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConfigCenterDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoSettingController : ControllerBase
    {
        private readonly ILogger<DemoSettingController> _logger;
        private readonly IConsulClient _consulClient;

        public DemoSettingController(
            ILogger<DemoSettingController> logger,
            IConsulClient consulClient)
        {
            _logger = logger;
            _consulClient = consulClient;
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] string key)
        {
            var settingModel = await _consulClient.GetValueAsync<SettingModel>(key);
            return settingModel.Message;
        }
    }
}