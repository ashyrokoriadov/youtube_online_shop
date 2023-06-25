using OnlineShop.Ui.Models.Security;

namespace OnlineShop.Ui.Security
{
    public class LoginStatus
    {
        public Token Token { get; set; } = new();
        public User User { get; set; } = new();
    }
}
