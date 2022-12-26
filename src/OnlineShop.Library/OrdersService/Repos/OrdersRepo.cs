using Microsoft.EntityFrameworkCore;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Data;
using OnlineShop.Library.OrdersService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Library.OrdersService.Repos
{
    public class OrdersRepo : BaseRepo<Order>
    {
        private readonly IRepo<OrderedArticle> _orderedArticlesRepo;

        public OrdersRepo(IRepo<OrderedArticle> orderedArticlesRepo, OrdersDbContext context) : base(context)
        {
            _orderedArticlesRepo = orderedArticlesRepo;
            Table = Context.Orders;
        }

        public override async Task<IEnumerable<Order>> GetAllAsync() => await Table.Include(nameof(Order.Articles)).ToListAsync();

        public override async Task<Order> GetOneAsync(Guid id) => await Task.Run(() => Table.Include(nameof(Order.Articles)).FirstOrDefault(entity => entity.Id == id));
    }
}
