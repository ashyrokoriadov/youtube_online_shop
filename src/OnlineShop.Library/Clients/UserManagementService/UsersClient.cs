using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineShop.Library.Constants;
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

        public async Task<IdentityResult> Add(CreateUserRequest request) 
            => await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{RepoActions.Add}");

        public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request) 
            => await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.ChangePassword}");

        public async Task<IdentityResult> AddToRole(AddRemoveRoleRequest request) 
            => await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.AddToRole}");

        public async Task<IdentityResult> AddToRoles(AddRemoveRolesRequest request) 
            => await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.AddToRoles}");

        public async Task<IdentityResult> RemoveFromRole(AddRemoveRoleRequest request) 
            => await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.RemoveFromRole}");

        public async Task<IdentityResult> RemoveFromRoles(AddRemoveRolesRequest request) 
            => await SendPostRequest(request, $"/{UsersControllerRoutes.ControllerName}/{UsersControllerRoutes.RemoveFromRoles}");

        public async Task<UserManagementServiceResponse<ApplicationUser>> Get(string name) 
            => await SendGetRequest<ApplicationUser>($"{UsersControllerRoutes.ControllerName}?name={name}");

        public async Task<UserManagementServiceResponse<IEnumerable<ApplicationUser>>> GetAll() 
            => await SendGetRequest<IEnumerable<ApplicationUser>>($"/{UsersControllerRoutes.ControllerName}/{RepoActions.GetAll}");

        public async Task<IdentityResult> Remove(ApplicationUser user) 
            => await SendPostRequest(user, $"/{UsersControllerRoutes.ControllerName}/{RepoActions.Remove}");       

        public async Task<IdentityResult> Update(ApplicationUser user) 
            => await SendPostRequest(user, $"/{UsersControllerRoutes.ControllerName}/{RepoActions.Update}");       
    }
}
