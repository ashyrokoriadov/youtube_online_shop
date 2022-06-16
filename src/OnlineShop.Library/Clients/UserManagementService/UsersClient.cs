using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineShop.Library.Options;
using OnlineShop.Library.UserManagementService.Models;
using OnlineShop.Library.UserManagementService.Requests;
using OnlineShop.Library.UserManagementService.Responses;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.UserManagementService
{
    public class UsersClient : UserManagementBaseClient, IUsersClient
    {       
        public UsersClient(HttpClient httpClient, IOptions<ServiceAdressOptions> options) : base(httpClient, options) { }

        public async Task<IdentityResult> Add(CreateUserRequest request) => await SendPostRequest(request, "/users/add");

        public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request) => await SendPostRequest(request, "/users/changepassword");

        public async Task<UserManagementServiceResponse<ApplicationUser>> Get(string name) => await SendGetRequest<ApplicationUser>($"users?name={name}");

        public async Task<UserManagementServiceResponse<IEnumerable<ApplicationUser>>> GetAll() => await SendGetRequest<IEnumerable<ApplicationUser>>("/users/all");

        public async Task<IdentityResult> Remove(ApplicationUser user) => await SendPostRequest(user, "/users/remove");       

        public async Task<IdentityResult> Update(ApplicationUser user) => await SendPostRequest(user, "/users/update");       
    }
}
