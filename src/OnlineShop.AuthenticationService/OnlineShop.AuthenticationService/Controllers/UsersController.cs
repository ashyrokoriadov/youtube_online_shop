using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Authentification.Models;
using OnlineShop.Library.Authentification.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("add")]
        public Task<IdentityResult> Add(CreateUserRequest request) 
        {
            var result = _userManager.CreateAsync(request.User, request.Password);
            return result;
        }

        [HttpPost("update")]
        public Task<IdentityResult> Update(ApplicationUser user)
        {
            var result = _userManager.UpdateAsync(user);
            return result;
        }

        [HttpPost("remove")]
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

        [HttpGet("all")]
        public IEnumerable<ApplicationUser> Get()
        {
            var result = _userManager.Users.AsEnumerable();
            return result;
        }
    }
}
