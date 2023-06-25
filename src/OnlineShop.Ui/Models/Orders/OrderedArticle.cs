namespace OnlineShop.Ui.Models.Orders
{
    public class OrderedArticle
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;

        public string PriceListName { get; set; } = string.Empty;

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
