using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Common.Repos;
using OnlineShop.Library.Constants;
using OnlineShop.Library.OrdersService.Models;
using OnlineShop.Library.OrdersService.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.OrdersService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : RepoControllerBase<Order>
    {
        public OrdersController(IOrdersRepo ordersRepo) : base(ordersRepo)
        { }

        [HttpPost(RepoActions.Update)]
        public override async Task<ActionResult> Update([FromBody] Order entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var entityToBeUpdated = await EntitiesRepo.GetOneAsync(entity.Id);
            if (entityToBeUpdated == null)
            {
                return BadRequest($"Entity with id = {entity.Id} was not found.");
            }

            var articlesId = entityToBeUpdated.Articles.Select(oa => oa.Id).ToArray();
            await ((IOrdersRepo)EntitiesRepo).DeleteArticlesFromOrderAsync(articlesId);

            UpdateProperties(entity, entityToBeUpdated);

            await EntitiesRepo.SaveAsync(entityToBeUpdated);
            return Ok(entityToBeUpdated);
        }

        [HttpPost(RepoActions.Remove)]
        public override async Task<ActionResult> Remove([FromBody] Guid id)
        {
            var result = await DeleteOrder(id);
            return result ? NoContent() : BadRequest($"Entity with id = {id} was not found.");
        }

        [HttpPost(RepoActions.RemoveRange)]
        public override async Task<ActionResult> Remove([FromBody] IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                var result = await DeleteOrder(id);
                if(!result)
                {
                    return BadRequest($"Entity with id = {id} was not found.");
                }
            }
            return NoContent();
        }

        protected override void UpdateProperties(Order source, Order destination)
        {
            destination.AddressId = source.AddressId;
            destination.UserId = source.UserId;
            destination.Articles = source.Articles;
            destination.Modified = DateTime.UtcNow;
        }

        private async Task<bool> DeleteOrder(Guid orderId)
        {
            var entityToBeRemoved = await EntitiesRepo.GetOneAsync(orderId);
            if (entityToBeRemoved == null)
            {
                return false;
            }

            var articlesId = entityToBeRemoved.Articles.Select(oa => oa.Id).ToArray();
            await((IOrdersRepo)EntitiesRepo).DeleteArticlesFromOrderAsync(articlesId);

            await EntitiesRepo.DeleteAsync(orderId);
            return true;
        }
    }
}
