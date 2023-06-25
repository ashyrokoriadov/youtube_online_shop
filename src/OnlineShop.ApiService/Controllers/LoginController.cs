using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Clients.UserManagementService;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.UserManagementService.Requests;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly ILoginClient _client;
        public LoginController(ILoginClient client) 
        {
            _client = client;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> LoginByUserNameAndPassword([FromForm] LoginRequest request)
        {
            var options = new IdentityServerUserNamePassword()
            {
                UserName = request.UserName,
                Password = request.Password
            };

            var token = await _client.GetApiTokenByUsernameAndPassword(options);
            return Ok(token);
        }
    }
}
