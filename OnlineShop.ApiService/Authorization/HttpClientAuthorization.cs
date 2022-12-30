using IdentityModel.Client;
using Microsoft.Extensions.Options;
using OnlineShop.Library.Clients;
using OnlineShop.Library.Clients.IdentityServer;
using OnlineShop.Library.Options;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.Authorization
{
    public class HttpClientAuthorization : IClientAuthorization
    {
        private readonly IIdentityServerClient _identityClient;
        private readonly IdentityServerApiOptions _identityServerApiOptions;

        public HttpClientAuthorization(
            IIdentityServerClient identityClient, 
            IOptions<IdentityServerApiOptions> options)
        {
            _identityServerApiOptions = options.Value;
            _identityClient = identityClient;
        }

        public async Task Authorize(IHttpClientContainer clientContainer)
        {
            if (clientContainer == null)
                return;

            var token = await _identityClient.GetApiToken(_identityServerApiOptions);
            clientContainer.HttpClient.SetBearerToken(token.AccessToken);
        }
    }
}
