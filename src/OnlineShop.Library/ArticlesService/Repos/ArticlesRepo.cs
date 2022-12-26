using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using OnlineShop.Library.ArticlesService.Models;

namespace OnlineShop.Library.ArticlesService.Repos
{
    public class ArticlesRepo : BaseRepo<Article>
    {
        public ArticlesRepo(OrdersDbContext context) : base(context)
        {
            Table = Context.Articles;
        }
    }
}
