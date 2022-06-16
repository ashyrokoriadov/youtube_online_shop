using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Authentification.Models;
using OnlineShop.Library.Authentification.Requests;
using OnlineShop.Library.Constants;
using System.Collections.Generic;
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
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost(RepoActions.Add)]
        public Task<IdentityResult> Add(CreateUserRequest request) 
        {
            var result = _userManager.CreateAsync(request.User, request.Password);
            return result;
        }

        [HttpPost(RepoActions.Update)]
        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var userToBeUpdated = await _userManager.FindByNameAsync(user.UserName);
            if (userToBeUpdated == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {user.UserName} was not found." });

            userToBeUpdated.DefaultAddress = user.DefaultAddress;
            userToBeUpdated.DeliveryAddress = user.DeliveryAddress;
            userToBeUpdated.PhoneNumber = user.PhoneNumber;
            userToBeUpdated.Email = user.Email;

            var result = await _userManager.UpdateAsync(userToBeUpdated);
            return result;
        }

        [HttpPost(RepoActions.Remove)]
        public Task<IdentityResult> Remove(ApplicationUser user)
        {
            var result = _userManager.DeleteAsync(user);
            return result;
        }

        [HttpGet]
        public Task<ApplicationUser> Get(string name)
        {
            var result = _userManager.FindByNameAsync(name);
            return result;
        }

        [HttpGet(RepoActions.GetAll)]
        public IEnumerable<ApplicationUser> Get()
        {
            var result = _userManager.Users.AsEnumerable();
            return result;
        }

        [HttpPost("changepassword")]
        public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return IdentityResult.Failed(new IdentityError() { Description = $"User {request.UserName} was not found."});

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            return result;
        }
    }
}
