using OnlineShop.Ui.Models.Articles;
using OnlineShop.Ui.Models.Orders;

namespace OnlineShop.Ui.Components.Abstractions
{
    public interface IOrdersManager
    {
        Task SubmitOrder(Guid addressId, Dictionary<Article, int> cartItems);

        Task<IEnumerable<OrderView>> GetAllOrders();

        event EventHandler? CartStatusHasChanged;
    }
}
