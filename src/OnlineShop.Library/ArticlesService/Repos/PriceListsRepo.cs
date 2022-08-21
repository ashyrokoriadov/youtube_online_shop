using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using OnlineShop.Library.ArticlesService.Models;

namespace OnlineShop.Library.ArticlesService.Repos
{
    public class PriceListsRepo : ArticlesBaseRepo<PriceList>
    {
        public PriceListsRepo(ArticlesDbContext context) : base(context)
        {
            Table = Context.PriceLists;
        }
    }
}
