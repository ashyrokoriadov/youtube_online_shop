using Microsoft.AspNetCore.Identity;
using OnlineShop.Library.Authentification.Models;
using OnlineShop.Library.Authentification.Requests;
using OnlineShop.Library.Authentification.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.AuthenticationService
{
    public interface IUsersClient
    {
        Task<IdentityResult> Add(CreateUserRequest request);

        Task<IdentityResult> Update(ApplicationUser user);

        Task<IdentityResult> Remove(ApplicationUser user);

        Task<AuthenticationServiceResponse<ApplicationUser>> Get(string name);

        Task<AuthenticationServiceResponse<IEnumerable<ApplicationUser>>> GetAll();

        Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request);
    }
}
