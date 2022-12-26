using Microsoft.EntityFrameworkCore;
using OnlineShop.Library.ArticlesService.Models;
using OnlineShop.Library.OrdersService.Models;

namespace OnlineShop.Library.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderedArticle>()
                .HasOne<Order>(e => e.Order)
                .WithMany(d => d.Articles)
                .HasForeignKey(e => e.OrderId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<PriceList> PriceLists { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderedArticle> OrderedArticles { get; set; }
    }
}
