using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.OrdersService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.OrdersService.Repos
{
    public interface IOrdersRepo : IRepo<Order>
    {
        Task<int> DeleteArticlesFromOrderAsync(IEnumerable<Guid> articlesId);
    }
}
