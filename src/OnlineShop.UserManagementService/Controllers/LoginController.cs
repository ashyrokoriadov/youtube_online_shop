using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.Logging;
using OnlineShop.Library.Options;
using System;
using System.Threading.Tasks;

namespace OnlineShop.UserManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IIdentityServerClient _identityServerClient;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IIdentityServerClient identityServerClient, ILogger<LoginController> logger)
        {
            _identityServerClient = identityServerClient;
            _logger = logger;
        }

        [HttpPost("ByClientSeceret")]
        public async Task<IActionResult> GetApiTokenByClientSeceret([FromBody] IdentityServerApiOptions options)
        {
            try
            {
                var token = await _identityServerClient.GetApiToken(options);
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(LoginController))
                   .WithMethod(nameof(GetApiTokenByClientSeceret))
                   .WithComment(ex.ToString())
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost("ByUsernameAndPassword")]
        public async Task<IActionResult> GetApiTokenByUsernameAndPassword([FromBody] IdentityServerUserNamePassword options)
        {
            try
            {
                var token = await _identityServerClient.GetApiToken(options);
                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(LoginController))
                   .WithMethod(nameof(GetApiTokenByClientSeceret))
                   .WithComment(ex.ToString())
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }
    }
}
