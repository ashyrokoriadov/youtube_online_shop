namespace OnlineShop.Library.UserManagementService.Models
{
    public class UserManagementServiceToken
    {
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public string TokenType { get; set; }

        public string Scope { get; set; }
    }
}
