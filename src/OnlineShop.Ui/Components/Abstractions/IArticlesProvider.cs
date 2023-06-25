using OnlineShop.Ui.Models.Articles;

namespace OnlineShop.Ui.Components.Abstractions
{
    public interface IArticlesProvider
    {
        Task<IEnumerable<Article>> GetArticles();
    }
}
