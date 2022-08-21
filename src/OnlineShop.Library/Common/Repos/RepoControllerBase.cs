using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Library.Common.Repos
{
    public abstract class RepoControllerBase<T> : ControllerBase where T : IIdentifiable
    {
        protected readonly IRepo<T> EntitiesRepo;
        public RepoControllerBase(IRepo<T> entitiesRepo)
        {
            EntitiesRepo = entitiesRepo;
        }

        [HttpPost(RepoActions.Add)]
        public async Task<ActionResult> Add([FromBody] T entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var articleId = await EntitiesRepo.AddAsync(entity);
            return Ok(articleId);
        }

        [HttpPost(RepoActions.AddRange)]
        public async Task<ActionResult> Add([FromBody] IEnumerable<T> entities)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var articleIds = await EntitiesRepo.AddRangeAsync(entities);
            return Ok(articleIds);
        }

        [HttpGet]
        public async Task<ActionResult> GetOne(Guid id)
        {
            var article = await EntitiesRepo.GetOneAsync(id);
            return Ok(article);
        }

        [HttpGet(RepoActions.GetAll)]
        public async Task<ActionResult> GetAll()
        {
            var articles = await EntitiesRepo.GetAllAsync();
            return Ok(articles);
        }

        [HttpPost(RepoActions.Remove)]
        public async Task<ActionResult> Remove([FromBody] Guid id)
        {
            await EntitiesRepo.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost(RepoActions.RemoveRange)]
        public async Task<ActionResult> Remove([FromBody] IEnumerable<Guid> ids)
        {
            await EntitiesRepo.DeleteRangeAsync(ids);
            return NoContent();
        }

        [HttpPost(RepoActions.Update)]
        public async Task<ActionResult> Update([FromBody] T entity)
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

            UpdateProperties(entity, entityToBeUpdated);

            await EntitiesRepo.SaveAsync(entityToBeUpdated);
            return Ok(entityToBeUpdated);
        }

        protected abstract void UpdateProperties(T source, T destination);
    }
}
