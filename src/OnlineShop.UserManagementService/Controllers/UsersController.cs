using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.Library.Constants;
using OnlineShop.Library.Logging;
using OnlineShop.Library.UserManagementService.Models;
using OnlineShop.Library.UserManagementService.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineShop.UserManagementService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<ApplicationUser> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost(RepoActions.Add)]
        public async Task<IActionResult> Add(CreateUserRequest request)
        {
            try
            {
                var result = await _userManager.CreateAsync(request.User, request.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(Add))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(request.User.Id)}: {request.User.Id}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(RepoActions.Update)]
        public async Task<IActionResult> Update(ApplicationUser user)
        {
            try
            {
                var userToBeUpdated = await _userManager.FindByNameAsync(user.UserName);
                if (userToBeUpdated == null)
                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = $"User {user.UserName} was not found." }));

                userToBeUpdated.FirstName = user.FirstName;
                userToBeUpdated.LastName = user.LastName;
                userToBeUpdated.DefaultAddress = user.DefaultAddress;
                userToBeUpdated.DeliveryAddress = user.DeliveryAddress;
                userToBeUpdated.PhoneNumber = user.PhoneNumber;
                userToBeUpdated.Email = user.Email;

                var result = await _userManager.UpdateAsync(userToBeUpdated);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(Update))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(user.Id)}: {user.Id}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(RepoActions.Remove)]
        public async Task<IActionResult> Remove(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.DeleteAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(Get))
                   .WithComment(ex.ToString())
                   .WithParameters($"User id: {user.Id}")
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
                var result = await _userManager.FindByNameAsync(name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
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
                _logger.LogInformation(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(Get))
                   .WithComment("Getting all users")
                   .WithOperation(RepoActions.GetAll)
                   .WithParameters(LoggingConstants.NoParameters)
                   .ToString()
                   );

                //throw new Exception("Ku-ku!");

                var result = _userManager.Users.AsEnumerable();

                _logger.LogInformation(new LogEntry()
                  .WithClass(nameof(UsersController))
                  .WithMethod(nameof(Get))
                  .WithComment($"Got {result.Count()} users")
                  .WithOperation(RepoActions.GetAll)
                  .WithParameters(LoggingConstants.NoParameters)
                  .ToString()
                  );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                    .WithClass(nameof(UsersController))
                    .WithMethod(nameof(Get))
                    .WithComment(ex.ToString())
                    .WithParameters(LoggingConstants.NoParameters)
                    .ToString()
                    );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(UsersControllerRoutes.ChangePassword)]
        public async Task<IActionResult> ChangePassword(UserPasswordChangeRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    var description = $"User {request.UserName} was not found.";

                    _logger.LogWarning(new LogEntry()
                       .WithClass(nameof(UsersController))
                       .WithMethod(nameof(ChangePassword))
                       .WithComment(description)
                       .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                       .ToString()
                       );

                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = description }));
                }

                var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                return Ok(result);            
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(ChangePassword))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(UsersControllerRoutes.AddToRole)]
        public async Task<IActionResult> AddToRole(AddRemoveRoleRequest request)
        {
            try 
            { 
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    var description = $"User {request.UserName} was not found.";

                    _logger.LogWarning(new LogEntry()
                       .WithClass(nameof(UsersController))
                       .WithMethod(nameof(AddToRole))
                       .WithComment(description)
                       .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                       .ToString()
                       );

                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = description }));
                }

                var result = await _userManager.AddToRoleAsync(user, request.RoleName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(AddToRole))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(UsersControllerRoutes.AddToRoles)]
        public async Task<IActionResult> AddToRoles(AddRemoveRolesRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    var description = $"User {request.UserName} was not found.";

                    _logger.LogWarning(new LogEntry()
                       .WithClass(nameof(UsersController))
                       .WithMethod(nameof(AddToRoles))
                       .WithComment(description)
                       .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                       .ToString()
                       );

                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = description }));
                }

                var result = await _userManager.AddToRolesAsync(user, request.RoleNames);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(AddToRoles))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(UsersControllerRoutes.RemoveFromRole)]
        public async Task<IActionResult> RemoveFromRole(AddRemoveRoleRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    var description = $"User {request.UserName} was not found.";

                    _logger.LogWarning(new LogEntry()
                       .WithClass(nameof(UsersController))
                       .WithMethod(nameof(RemoveFromRole))
                       .WithComment(description)
                       .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                       .ToString()
                       );

                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = description }));
                }

                var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(RemoveFromRole))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }

        [HttpPost(UsersControllerRoutes.RemoveFromRoles)]
        public async Task<IActionResult> RemoveFromRoles(AddRemoveRolesRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    var description = $"User {request.UserName} was not found.";

                    _logger.LogWarning(new LogEntry()
                       .WithClass(nameof(UsersController))
                       .WithMethod(nameof(RemoveFromRole))
                       .WithComment(description)
                       .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                       .ToString()
                       );

                    return BadRequest(IdentityResult.Failed(new IdentityError() { Description = description }));
                }

                var result = await _userManager.RemoveFromRolesAsync(user, request.RoleNames);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(new LogEntry()
                   .WithClass(nameof(UsersController))
                   .WithMethod(nameof(RemoveFromRole))
                   .WithComment(ex.ToString())
                   .WithParameters($"{nameof(request.UserName)}: {request.UserName}")
                   .ToString()
                   );

                return StatusCode(500, LoggingConstants.InternalServerErrorMessage);
            }
        }
    }
}
