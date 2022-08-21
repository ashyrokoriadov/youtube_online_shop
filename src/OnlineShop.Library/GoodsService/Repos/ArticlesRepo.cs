using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using OnlineShop.Library.GoodsService.Models;

namespace OnlineShop.Library.GoodsService.Repos
{
    public class ArticlesRepo : ArticlesBaseRepo<Article>
    {
        public ArticlesRepo(ArticlesDbContext context) : base(context)
        {
            Table = Context.Articles;
        }
    }
}
