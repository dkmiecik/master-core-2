using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace test.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IsAliveController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
