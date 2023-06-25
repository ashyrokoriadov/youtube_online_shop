using OnlineShop.Ui.Models.Common;

namespace OnlineShop.Ui.Models.Security
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public Address DefaultAddress { get; set; } = new();

        public Address DeliveryAddress { get; set; } = new();
    }
}
