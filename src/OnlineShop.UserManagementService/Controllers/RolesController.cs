using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.Library.Constants;
using OnlineShop.Library.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.UserManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UsersController> _logger;
        public RolesController(RoleManager<IdentityRole> roleManager, ILogger<UsersController> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost(RepoActions.Add)]
        public async Task<IActionResult> Add(IdentityRole role)
        {
            try
            {
                var result = await _roleManager.CreateAsync(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(RolesController))
                   .WithMethod(nameof(Add))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(role.Name)}: {role.Name}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(RepoActions.Update)]
        public async Task<IActionResult> Update(IdentityRole role)
        {
            try
            {
                var roleToBeUpdated = await _roleManager.FindByIdAsync(role.Id);
                if (roleToBeUpdated == null)
                {
                    var description = $"Role {role.Name} was not found.";

                    _logger.LogWarning(new LogEntry()
                       .WithClass(nameof(RolesController))
                       .WithMethod(nameof(Update))
                       .WithComment(description)
                       .WithParameters($"{nameof(role.Name)}: {role.Name}")
                       .ToString()
                       );

                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = $"Role {role.Name} was not found." }));
                }

                roleToBeUpdated.Name = role.Name;
                var result = await _roleManager.UpdateAsync(roleToBeUpdated);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(RolesController))
                   .WithMethod(nameof(Get))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(role.Name)}: {role.Name}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(RepoActions.Remove)]
        public async Task<IActionResult> Remove(IdentityRole role)
        {
            try
            {
                var result = await _roleManager.DeleteAsync(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(RolesController))
                   .WithMethod(nameof(Get))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(role.Name)}: {role.Name}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                var result = await _roleManager.FindByNameAsync(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(RolesController))
                   .WithMethod(nameof(Get))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(name)}: {name}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpGet(RepoActions.GetAll)]
        public IActionResult Get()
        {
            try
            {
                var result = _roleManager.Roles.AsEnumerable();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(RolesController))
                   .WithMethod(nameof(Get))
                   .WithComment(ex.ToString())
                   .WithParameters(LoggingConstants.NoParameters)
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }
    }
}
