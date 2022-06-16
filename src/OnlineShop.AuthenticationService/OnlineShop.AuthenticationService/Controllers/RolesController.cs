using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost(RepoActions.Add)]
        public Task<IdentityResult> Add(IdentityRole role)
        {
            var result = _roleManager.CreateAsync(role);
            return result;
        }

        [HttpPost(RepoActions.Update)]
        public Task<IdentityResult> Update(IdentityRole role)
        {
            var result = _roleManager.UpdateAsync(role);
            return result;
        }

        [HttpPost(RepoActions.Remove)]
        public Task<IdentityResult> Remove(IdentityRole role)
        {
            var result = _roleManager.DeleteAsync(role);
            return result;
        }

        [HttpGet]
        public Task<IdentityRole> Get(string name)
        {
            var result = _roleManager.FindByNameAsync(name);
            return result;
        }

        [HttpGet(RepoActions.GetAll)]
        public IEnumerable<IdentityRole> Get()
        {
            var result = _roleManager.Roles.AsEnumerable();
            return result;
        }
    }
}
