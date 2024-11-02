using OnlineShop.Ui.Models.Common;
using OnlineShop.Ui.Models.Security;

namespace OnlineShop.Ui.Models.Orders
{
    public class OrderView
    {
        public Guid Id { get; set; }

        public Address Address { get; set; } = new();

        public User User { get; set; } = new();

        public DateTime Created { get; set; } 

        public DateTime Modified { get; set; }

        public List<OrderedArticle> Articles { get; set; } = new List<OrderedArticle>();
    }
}
