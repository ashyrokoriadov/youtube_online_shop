using Microsoft.JSInterop;
using Newtonsoft.Json;
using OnlineShop.Ui.Components.Abstractions;
using OnlineShop.Ui.Models.Articles;
using OnlineShop.Ui.Models.Orders;
using System.Net.Http.Json;

namespace OnlineShop.Ui.Components
{
    public class OrdersManager : IOrdersManager
    {
        private readonly HttpClient _client;
        private readonly ILoginStatusManager _loginStatusManager;
        private readonly IJSRuntime _jsRuntime;

        public OrdersManager(HttpClient client, ILoginStatusManager loginStatusManager, IJSRuntime jsRuntime)
        {
            _client = client;
            _loginStatusManager = loginStatusManager;
            _jsRuntime = jsRuntime;
        }

        public async Task SubmitOrder(Guid addressId, Dictionary<Article, int> cartItems)
        {
            var priceLists = await _client.GetFromJsonAsync<List<PriceList>>("PriceLists/all") ?? new();

            var order = new Order()
            {
                UserId = _loginStatusManager.LoginStatus.User.Id,
                AddressId = addressId,
                Articles = cartItems.Select(a => new OrderedArticle
                {
                    Name = a.Key.Name,
                    Description = a.Key.Description,
                    Price = a.Key.Price,
                    Quantity = a.Value,
                    PriceListName = priceLists.FirstOrDefault(pl => pl.ArticleId == a.Key.Id)?.Name ?? "Default",
                    ValidFrom = priceLists.FirstOrDefault(pl => pl.ArticleId == a.Key.Id)?.ValidFrom ?? DateTime.MinValue,
                    ValidTo = priceLists.FirstOrDefault(pl => pl.ArticleId == a.Key.Id)?.ValidTo ?? DateTime.MinValue
                }).ToList()
            };

            var jsonContent = JsonConvert.SerializeObject(order);
            var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            var requestResult = await _client.PostAsync("Orders/Add", httpContent);

            if (requestResult.IsSuccessStatusCode)
            {
                cartItems.Clear();
                CartStatusHasChanged?.Invoke(this, new EventArgs());
                await _jsRuntime.InvokeVoidAsync("alert", "Your order has been submitted. Thank you!");
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", $"Failed to send a request. Response: {requestResult.StatusCode}");
            }
        }
          
        public async Task<IEnumerable<OrderView>> GetAllOrders()
        {
            var orders = await _client.GetFromJsonAsync<List<Order>>("Orders/All") ?? new List<Order>();
            var userOrders = orders.Where(o => o.UserId == _loginStatusManager.LoginStatus.User.Id);
            var result = userOrders.Select(o => new OrderView()
            {
                Id = o.Id,
                User = _loginStatusManager.LoginStatus.User,
                Address = _loginStatusManager.LoginStatus.User.DeliveryAddress,
                Created = o.Created,
                Modified = o.Modified,
                Articles = o.Articles
            });
            return result;
        }

        public event EventHandler? CartStatusHasChanged;
    }
}
