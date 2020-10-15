using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Mvc.Assets
{
    [ApiController]
    [Route("[controller]")]
    public class FakeController : ControllerBase
    {
        [HttpGet]
        [BearerThrottlingSentinel(10, 5, TimeUnit.Seconds)]
        public IActionResult Get()
        {
            return Ok("Unit Test");
        }

        [HttpGet("it")]
        public IActionResult GetIt()
        {
            return Ok("Unit Test");
        }

        [HttpGet("oneSecond")]
        public async Task<IActionResult> GetAfter1Second()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return Ok("Unit Test");
        }

        [ServerTiming(Name = "action-result")]
        [HttpGet("oneSecondAttribute")]
        public async Task<IActionResult> GetAfter1SecondDecorated()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return Ok("Unit Test");
        }
    }
}