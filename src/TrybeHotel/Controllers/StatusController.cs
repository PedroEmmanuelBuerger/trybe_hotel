using Microsoft.AspNetCore.Mvc;


namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("/")]
    public class StatusController : Controller
    {
         [HttpGet]
        public IActionResult GetStatus()
        {
            return StatusCode(200, new { message  = "online" });
        }
    }
}
