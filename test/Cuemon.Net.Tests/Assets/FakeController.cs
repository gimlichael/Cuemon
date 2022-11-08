using System;
using Cuemon.Extensions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Net.Assets
{
    [ApiController]
    [Route("[controller]")]
    public class FakeController : ControllerBase
    {
        [HttpHead]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Guid.NewGuid().ToString("N").WithLastModifiedHeader(o => o.TimestampProvider = _ => DateTime.UtcNow));
        }
    }
}