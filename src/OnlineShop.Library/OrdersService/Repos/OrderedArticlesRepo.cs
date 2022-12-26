using Microsoft.EntityFrameworkCore;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.OrdersService.Repos
{
    public class OrderedArticlesRepo : BaseRepo<OrderedArticle>
    {
        public OrderedArticlesRepo(OrdersDbContext context) : base(context)
        {
            Table = Context.OrderedArticles;
        }
    }
}
