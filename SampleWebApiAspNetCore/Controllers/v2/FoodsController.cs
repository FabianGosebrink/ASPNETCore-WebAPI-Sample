using Microsoft.AspNetCore.Mvc;

namespace SampleWebApiAspNetCore.v2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FoodsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("2.0");
        }
    }
}
