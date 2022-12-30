using OnlineShop.Library.Clients;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.Authorization
{
    public interface IClientAuthorization
    {
        Task Authorize(IHttpClientContainer clientContainer);
    }
}
