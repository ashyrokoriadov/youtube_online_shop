using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Library.OrdersService.Models
{
    public class Order : IIdentifiable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }

        [Required]
        public Guid AddressId { get; set; }

        [Required]
        [Column(TypeName = "uniqueidentifier")]
        public Guid UserId { get; set; }

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime Created { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Modified { get; set; }

        public List<OrderedArticle> Articles { get; set; }
    }
}
