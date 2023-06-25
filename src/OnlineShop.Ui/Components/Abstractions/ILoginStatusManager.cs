using OnlineShop.Ui.Security;

namespace OnlineShop.Ui.Components.Abstractions
{
    public interface ILoginStatusManager
    {
        Task<bool> LogIn(string username, string password);
        
        Task<bool> LogOut();

        LoginStatus LoginStatus { get; }

        public event EventHandler LoginStatusHasChanged;
    }
}
