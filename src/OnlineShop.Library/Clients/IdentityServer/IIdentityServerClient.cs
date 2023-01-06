using OnlineShop.Library.Common.Models;
using OnlineShop.Library.IdentityServer;
using OnlineShop.Library.Options;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.IdentityServer
{
    public interface IIdentityServerClient
    {
        Task<Token> GetApiToken(IdentityServerApiOptions options);

        Task<Token> GetApiToken(IdentityServerUserNamePassword options);
    }
}
