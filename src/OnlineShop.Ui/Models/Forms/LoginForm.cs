using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Ui.Models.Forms
{
    public class LoginForm
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }
}
