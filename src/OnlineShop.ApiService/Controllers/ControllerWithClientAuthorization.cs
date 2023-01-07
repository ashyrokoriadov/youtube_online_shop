using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Authorization;
using OnlineShop.Library.Clients;

namespace OnlineShop.ApiService.Controllers
{
    public abstract class ControllerWithClientAuthorization<TClient> : ControllerBase
    {
        protected readonly TClient Client;

        public ControllerWithClientAuthorization(TClient client, IClientAuthorization clientAuthorization)
        {
            Client = client;

            if (Client is IHttpClientContainer clientContainer)
            {
                clientAuthorization.Authorize(clientContainer).Wait();
            }
        }
    }
}
