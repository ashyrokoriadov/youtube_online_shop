using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Common.Repos;

namespace OnlineShop.OrdersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderedArticlesController : RepoControllerBase<OrderedArticle>
    {
        public OrderedArticlesController(IRepo<OrderedArticle> ordersRepo, ILogger<OrderedArticlesController> logger) : base(ordersRepo, logger)
        { }

        protected override void UpdateProperties(OrderedArticle source, OrderedArticle destination)
        {
            destination.Name = source.Name;
            destination.Description = source.Description;

            if(destination.Price != source.Price)
            {
                destination.PriceListName = "Manualy assigned";
            }

            destination.Price = source.Price;
            destination.Quantity = source.Quantity;
        }
    }
}
