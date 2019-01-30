using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetCoreWithCAP.Controllers
{
    public class RabbitController : Controller
    {
        private readonly ICapPublisher _capBus;

        public RabbitController(ICapPublisher capPublisher)
        {
            this._capBus = capPublisher;
        }

        [Route("~/without/transaction")]
        public IActionResult WithoutTransaction()
        {
            _capBus.Publish("xxx.services.show.time", DateTime.Now);
            return Ok();
        }
    }
}
