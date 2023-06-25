using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Ui.Models.Forms
{
    public class AddressForm
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "PostalCode is required.")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "AddressLine1 is required.")]
        public string AddressLine1 { get; set; } = string.Empty;

        public string AddressLine2 { get; set; } = string.Empty;
    }
}
