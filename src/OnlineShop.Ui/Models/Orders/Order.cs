namespace OnlineShop.Ui.Models.Orders
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid AddressId { get; set; }

        public Guid UserId { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public DateTime Modified { get; set; }

        public List<OrderedArticle> Articles { get; set; } = new List<OrderedArticle>();
    }
}
