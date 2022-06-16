namespace OnlineShop.Library.Authentification.Requests
{
    public class UserPasswordChangeRequest
    {
        public string UserName { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
