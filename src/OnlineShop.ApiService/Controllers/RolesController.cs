using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Authorization;
using OnlineShop.Library.Clients.UserManagementService;
using OnlineShop.Library.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RolesController : ControllerWithClientAuthorization<IRolesClient>
    {
        public RolesController(IRolesClient rolesClient, IClientAuthorization clientAuthorization) : base (rolesClient, clientAuthorization)
        { }

        [HttpPost(RepoActions.Add)]
        public Task<IdentityResult> Add(IdentityRole role)
        {
            var result = Client.Add(role);
            return result;
        }

        [HttpPost(RepoActions.Update)]
        public async Task<IdentityResult> Update(IdentityRole role)
        {
            var result = await Client.Update(role);
            return result;
        }

        [HttpPost(RepoActions.Remove)]
        public async Task<IdentityResult> Remove(IdentityRole role)
        {
            var result = await Client.Remove(role);
            return result;
        }

        [HttpGet]
        public async Task<IdentityRole> Get(string name)
        {
            var result = await Client.Get(name);
            return result.Payload;
        }

        [HttpGet(RepoActions.GetAll)]
        public async Task<IEnumerable<IdentityRole>> Get()
        {
            var result = await Client.GetAll();
            return result.Payload;
        }        
    }
}
