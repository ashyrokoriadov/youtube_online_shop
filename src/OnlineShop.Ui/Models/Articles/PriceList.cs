namespace OnlineShop.Ui.Models.Articles
{
    public class PriceList 
    {
        public Guid Id { get; set; }

        public Guid ArticleId { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
