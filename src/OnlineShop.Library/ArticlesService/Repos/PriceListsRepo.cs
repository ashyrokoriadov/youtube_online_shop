using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using OnlineShop.Library.ArticlesService.Models;

namespace OnlineShop.Library.ArticlesService.Repos
{
    public class PriceListsRepo : BaseRepo<PriceList>
    {
        public PriceListsRepo(OrdersDbContext context) : base(context)
        {
            Table = Context.PriceLists;
        }
    }
}
