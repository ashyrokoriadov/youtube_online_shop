using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineShop.Library.Options;
using OnlineShop.Library.UserManagementService.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.UserManagementService
{
    public class RolesClient : UserManagementBaseClient, IRolesClient
    {
        public RolesClient(HttpClient httpClient, IOptions<ServiceAdressOptions> options) : base(httpClient, options) { }

        public async Task<IdentityResult> Add(IdentityRole role) => await SendPostRequest(role, "/roles/add");

        public async Task<UserManagementServiceResponse<IdentityRole>> Get(string name) => await SendGetRequest<IdentityRole>($"roles?name={name}");

        public async Task<UserManagementServiceResponse<IEnumerable<IdentityRole>>> GetAll() => await SendGetRequest<IEnumerable<IdentityRole>>("/roles/all");

        public async Task<IdentityResult> Remove(IdentityRole role) => await SendPostRequest(role, "/roles/remove");

        public async Task<IdentityResult> Update(IdentityRole role) => await SendPostRequest(role, "/roles/update");
    }
}
