using OnlineShop.Library.UserManagementService.Models;

namespace OnlineShop.Library.UserManagementService.Requests
{
    public class AddRemoveRoleRequest
    {
        public ApplicationUser User { get; set; }

        public string RoleName { get; set; }
    }
}
