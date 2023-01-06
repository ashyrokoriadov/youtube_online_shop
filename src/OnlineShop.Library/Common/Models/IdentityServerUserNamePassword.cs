using OnlineShop.Library.Constants;

namespace OnlineShop.Library.Common.Models
{
    public class IdentityServerUserNamePassword
    {
        public string ClientId { get; set; } = "external";
        public string Scope { get; set; } = IdConstants.WebScope;
        public string GrantType { get; set; } = IdConstants.GrantType_Password;
        public string UserName { get; set; } 
        public string Password { get; set; } 
    }
}
