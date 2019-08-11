using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PocIdentityServer.Application
{
    [Route("api/v1")]
    public class IdentityController : ControllerBase
    {
        [Authorize]
        [HttpGet("identity")]
        public IActionResult Get()
        {
            return new OkObjectResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}