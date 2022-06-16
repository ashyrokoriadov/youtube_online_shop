using Microsoft.AspNetCore.Identity;
using OnlineShop.Library.Common.Models;

namespace OnlineShop.Library.UserManagementService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Address DefaultAddress { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}
