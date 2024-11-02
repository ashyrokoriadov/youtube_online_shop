using Microsoft.JSInterop;
using OnlineShop.Ui.Components.Abstractions;
using OnlineShop.Ui.Models.Articles;

namespace OnlineShop.Ui.States
{
    public class CartState
    {
        private readonly IArticlesProvider _articlesProvider;
        private readonly IJSRuntime _jSRuntime;

        public Dictionary<Article, int> Items { get; set; } = new();

        public CartState(IArticlesProvider articlesProvider, IJSRuntime jSRuntime)
        {
            _articlesProvider = articlesProvider;
            _jSRuntime = jSRuntime;
        }

        public async Task AddArticleToCartAsync(Guid articleId, int quantity)
        {
            if (Items.Any(p => p.Key.Id == articleId) is false)
            {
                var articles = await _articlesProvider.GetArticles();
                var article = articles.FirstOrDefault(p => p.Id == articleId);

                if (article != null && quantity > 0)
                {
                    Items.Add(article, quantity);
                    await _jSRuntime.InvokeVoidAsync("alert", $"{article.Name} was added to a cart. Click Checkout button to see items in the cart.");
                }
            }
        }
    }
}
