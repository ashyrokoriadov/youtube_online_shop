using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Authorization;
using OnlineShop.Library.Clients.UserManagementService;
using OnlineShop.Library.Constants;
using OnlineShop.Library.UserManagementService.Models;
using OnlineShop.Library.UserManagementService.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : ControllerWithClientAuthorization<IUsersClient>
    {
        public UsersController(IUsersClient usersClient, IClientAuthorization clientAuthorization) : base (usersClient, clientAuthorization)
        {  }

        [HttpPost(RepoActions.Add)]
        public Task<IdentityResult> Add(CreateUserRequest request)
        {
            var result = Client.Add(request);
            return result;
        }

        [HttpPost(RepoActions.Update)]
        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            var result = await Client.Update(user);
            return result;
        }

        [HttpPost(RepoActions.Remove)]
        public Task<IdentityResult> Remove(ApplicationUser user)
        {
            var result = Client.Remove(user);
            return result;
        }

        [HttpGet]
        public async Task<ApplicationUser> Get(string name)
        {
            var result = await Client.Get(name);
            return result.Payload;
        }

        [HttpGet(RepoActions.GetAll)]
        public async Task<IEnumerable<ApplicationUser>> Get()
        {
            var result = await Client.GetAll();
            return result.Payload;
        }

        [HttpPost(UsersControllerRoutes.ChangePassword)]
        public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request)
        {
            var result = await Client.ChangePassword(request);
            return result;
        }

        [HttpPost(UsersControllerRoutes.AddToRole)]
        public async Task<IdentityResult> AddToRole(AddRemoveRoleRequest request)
        {
            var result = await Client.AddToRole(request);
            return result;
        }

        [HttpPost(UsersControllerRoutes.AddToRoles)]
        public async Task<IdentityResult> AddToRoles(AddRemoveRolesRequest request)
        {
            var result = await Client.AddToRoles(request);
            return result;
        }

        [HttpPost(UsersControllerRoutes.RemoveFromRole)]
        public async Task<IdentityResult> RemoveFromRole(AddRemoveRoleRequest request)
        {
            var result = await Client.RemoveFromRole(request);
            return result;
        }

        [HttpPost(UsersControllerRoutes.RemoveFromRoles)]
        public async Task<IdentityResult> RemoveFromRoles(AddRemoveRolesRequest request)
        {
            var result = await Client.AddToRoles(request);
            return result;
        }
    }
}
