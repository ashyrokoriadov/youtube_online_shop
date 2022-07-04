using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.Common.Models
{
    [Table("Addresses")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public string Country { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public string City { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(32)")]
        public string PostalCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(256)")]
        public string AddressLine1 { get; set; }

        [Column(TypeName = "nvarchar(256)")]
        public string AddressLine2 { get; set; }
    }
}
