using OnlineShop.Library.UserManagementService.Models;

namespace OnlineShop.Library.UserManagementService.Requests
{
    public class AddRemoveRolesRequest
    {
        public ApplicationUser User { get; set; }

        public string[] RoleNames { get; set; }
    }
}
