using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore.Assets.V1
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class FakeController : ControllerBase
    {
        /// <summary>
        /// Gets an OK response with a body of Unit Test V1.
        /// </summary>
        /// <returns>IActionResult.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Unit Test V1");
        }
    }
}
