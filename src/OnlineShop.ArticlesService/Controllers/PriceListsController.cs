using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Common.Repos;

namespace OnlineShop.ArticlesService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PriceListsController : RepoControllerBase<PriceList>
    {
        public PriceListsController(IRepo<PriceList> priceListsRepo) : base(priceListsRepo)
        { }

        protected override void UpdateProperties(PriceList source, PriceList destination)
        {
            destination.Price = source.Price;
            destination.Name = source.Name;
            destination.ValidFrom = source.ValidFrom;
            destination.ValidTo = source.ValidTo;
        }
    }
}
