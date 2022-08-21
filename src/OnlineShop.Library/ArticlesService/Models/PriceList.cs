using OnlineShop.Library.Common.Interfaces;
using OnlineShop.Library.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Library.ArticlesService.Models
{
    public class PriceList : IIdentifiable
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Article")]
        public Guid ArticleId { get; set; }

        public decimal Value { get; set; }

        [Required]
        public string Name { get; set; } = PriceListNames.DEFAULT;

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime ValidFrom { get; set; }

        [Column(TypeName = "datetime2"), Required]
        public DateTime ValidTo { get; set; }
    }
}
