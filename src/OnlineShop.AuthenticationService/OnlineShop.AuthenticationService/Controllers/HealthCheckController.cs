using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string Check() => "Service is online";
    }
}
