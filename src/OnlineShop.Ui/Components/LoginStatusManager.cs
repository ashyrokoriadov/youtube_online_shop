using OnlineShop.Ui.Components.Abstractions;
using OnlineShop.Ui.Models.Security;
using OnlineShop.Ui.Security;
using System.Net.Http.Json;

namespace OnlineShop.Ui.Components
{
    public class LoginStatusManager : ILoginStatusManager
    {
        private readonly HttpClient _client;
        private LoginStatus _loginStatus = new();

        public LoginStatusManager(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> LogIn(string username, string password)
        {
            try
            {
                var content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("UserName", username),
                new KeyValuePair<string, string>("Password", password) }
                );

                var response = await _client.PostAsync("Login", content);
                if (response.IsSuccessStatusCode)
                {
                    _loginStatus.Token = await response.Content.ReadFromJsonAsync<Token>() ?? new();
                }
                else
                {
                    return false;
                }

                _client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer",
                        _loginStatus.Token.AccessToken);

                _loginStatus.User = await _client.GetFromJsonAsync<User>($"users?name={username}") ?? new();

                LoginStatusHasChanged?.Invoke(this, new EventArgs());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LogOut()
        {            
            _loginStatus.Token = new();                
            _loginStatus.User = new();

            LoginStatusHasChanged?.Invoke(this, new EventArgs());

            return await Task.FromResult(true);            
        }

        public LoginStatus LoginStatus => _loginStatus;

        public event EventHandler? LoginStatusHasChanged;
    }
}
