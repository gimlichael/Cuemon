using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Mvc.Filters.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.Asp.Versioning.Assets
{
    [ApiController]
    [Route("[controller]")]
    public class FakeController : ControllerBase
    {
        private readonly MvcFaultDescriptorOptions _options;

        public FakeController(IOptions<MvcFaultDescriptorOptions> setup)
        {
            _options = setup.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Unit Test");
        }

        [HttpGet]
        [Route("throw")]
        public IActionResult GetException()
        {
            throw new GoneException();
        }
    }
}
