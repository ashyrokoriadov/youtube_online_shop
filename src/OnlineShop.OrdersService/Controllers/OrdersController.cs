using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.OrdersService.Models;
using System;

namespace OnlineShop.OrdersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
	[Authorize(AuthenticationSchemes = "Bearer")]
    public class OrdersController : RepoControllerBase<Order>
    {
        public OrdersController(IRepo<Order> ordersRepo) : base(ordersRepo)
        { }
               
        protected override void UpdateProperties(Order source, Order destination)
        {
            destination.AddressId = source.AddressId;
            destination.UserId = source.UserId;
            destination.Articles = source.Articles;
            destination.Modified = DateTime.UtcNow;
        }
    }
}
