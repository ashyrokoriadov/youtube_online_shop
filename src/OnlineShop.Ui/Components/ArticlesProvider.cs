using OnlineShop.Ui.Components.Abstractions;
using OnlineShop.Ui.Models.Articles;
using System.Net.Http.Json;

namespace OnlineShop.Ui.Components
{
    public class ArticlesProvider : IArticlesProvider
    {
        private readonly HttpClient _client;
        public ArticlesProvider(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            var articles = await _client.GetFromJsonAsync<List<Article>>("Articles/all") ?? new();
            var priceLists = await _client.GetFromJsonAsync<List<PriceList>>("PriceLists/all") ?? new();

            foreach (var article in articles)
            {
                var price = priceLists.FirstOrDefault(price => price.ArticleId == article.Id)?.Price ?? 0M;
                article.Price = price;
            }

            return articles;
        }
    }
}
