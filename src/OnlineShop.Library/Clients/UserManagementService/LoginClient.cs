using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineShop.Library.Common.Models;
using OnlineShop.Library.Options;
using OnlineShop.Library.UserManagementService.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.UserManagementService
{
    public class LoginClient : UserManagementBaseClient, ILoginClient
    {
        public LoginClient(HttpClient httpClient, IOptions<ServiceAdressOptions> options) : base(httpClient, options) { }

        public async Task<UserManagementServiceToken> GetApiTokenByClientSeceret(IdentityServerApiOptions options)
            => await SendPostRequest(options, "/Login/ByClientSeceret");

        public async Task<UserManagementServiceToken> GetApiTokenByUsernameAndPassword(IdentityServerUserNamePassword options)
            => await SendPostRequest(options, "/Login/ByUsernameAndPassword");

        private new async Task<UserManagementServiceToken> SendPostRequest<T>(T options, string path)
        {
            var jsonContent = JsonConvert.SerializeObject(options);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var requestResult = await HttpClient.PostAsync(path, httpContent);

            if (requestResult.IsSuccessStatusCode)
            {
                var responseJson = await requestResult.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<UserManagementServiceToken>(responseJson);
                return response;
            }

            return null;
        }
    }
}
