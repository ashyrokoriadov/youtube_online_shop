using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.ApiService.Authorization;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Clients;
using OnlineShop.Library.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ArticlesController : ControllerWithClientAuthorization<IRepoClient<Article>>
    {
        public ArticlesController(IRepoClient<Article> client, IClientAuthorization clientAuthorization) : base(client, clientAuthorization)
        { }

        [HttpPost(RepoActions.Add)]
        public async Task<ActionResult> Add([FromBody] Article entity)
        {
            var response = await Client.Add(entity);
            return Ok(response.Payload);
        }

        [HttpPost(RepoActions.AddRange)]
        public async Task<ActionResult> Add([FromBody] IEnumerable<Article> entities)
        {
            var response = await Client.AddRange(entities);
            return Ok(response.Payload);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetOne(Guid id)
        {
            var response = await Client.GetOne(id);
            return Ok(response.Payload);
        }

        [HttpGet(RepoActions.GetAll)]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            var response = await Client.GetAll();
            return Ok(response.Payload);
        }

        [HttpPost(RepoActions.Remove)]
        public virtual async Task<ActionResult> Remove([FromBody] Guid id)
        {
            await Client.Remove(id);
            return NoContent();
        }

        [HttpPost(RepoActions.RemoveRange)]
        public virtual async Task<ActionResult> Remove([FromBody] IEnumerable<Guid> ids)
        {
            await Client.RemoveRange(ids);
            return NoContent();
        }

        [HttpPost(RepoActions.Update)]
        public virtual async Task<ActionResult> Update([FromBody] Article entity)
        {
            var response = await Client.Update(entity);
            return Ok(response.Payload);
        }
    }
}
