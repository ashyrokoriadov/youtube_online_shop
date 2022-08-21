using OnlineShop.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Library.ArticlesService.Models
{
    public class Article : IIdentifiable
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<PriceList> PriceLists { get; set; }
    }
}
