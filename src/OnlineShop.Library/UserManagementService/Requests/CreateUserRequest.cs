using OnlineShop.Library.Authentification.Models;

namespace OnlineShop.Library.Authentification.Requests
{
    public class CreateUserRequest
    {
        public ApplicationUser User { get; set; }

        public string Password { get; set; }
    }
}
