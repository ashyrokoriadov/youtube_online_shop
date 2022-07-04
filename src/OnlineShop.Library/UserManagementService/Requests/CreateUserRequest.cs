using OnlineShop.Library.UserManagementService.Models;

namespace OnlineShop.Library.UserManagementService.Requests
{
    public class CreateUserRequest
    {
        public ApplicationUser User { get; set; }

        public string Password { get; set; }
    }
}
