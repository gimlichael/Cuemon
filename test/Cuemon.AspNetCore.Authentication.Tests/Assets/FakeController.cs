using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.AspNetCore.Authentication.Assets
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FakeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Unit Test");
        }

        [AllowAnonymous]
        [HttpGet("anonymous")]
        public IActionResult GetAnonymous()
        {
	        return Ok("Unit Test");
        }
    }
}
