using Microsoft.AspNetCore.Mvc;

namespace EventManagmentApi.Controllers
{

    [Route("api/[controller]")]
    public class TestingController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("✅ API is working!");
        }
    }
}
