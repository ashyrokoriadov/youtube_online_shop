using Microsoft.Extensions.Options;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Options;
using System;
using System.Net.Http;

namespace OnlineShop.Library.Clients.OrdersService
{
    public class OrderedArticlesClient : RepoClientBase<OrderedArticle>
    {
        public OrderedArticlesClient(HttpClient client, IOptions<ServiceAdressOptions> options) : base(client, options)
        { }

        protected override void InitializeClient(IOptions<ServiceAdressOptions> options)
        {
            HttpClient.BaseAddress = new Uri(options.Value.OrdersService);
        }

        protected override void SetControllerName()
        {
            ControllerName = "OrderedArticles";
        }
    }
}
