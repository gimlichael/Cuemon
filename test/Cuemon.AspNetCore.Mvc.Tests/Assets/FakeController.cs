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
    }
}