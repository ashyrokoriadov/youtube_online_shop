using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using OnlineShop.Library.GoodsService.Models;

namespace OnlineShop.Library.GoodsService.Repos
{
    public class PriceListsRepo : ArticlesBaseRepo<PriceList>
    {
        public PriceListsRepo(ArticlesDbContext context) : base(context)
        {
            Table = Context.PriceLists;
        }
    }
}
