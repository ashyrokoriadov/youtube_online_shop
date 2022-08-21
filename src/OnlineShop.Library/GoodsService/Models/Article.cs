using OnlineShop.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Library.GoodsService.Models
{
    public class Article : IIdentifiable
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PriceList> PriceLists { get; set; }
    }
}
