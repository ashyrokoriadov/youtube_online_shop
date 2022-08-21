using Microsoft.EntityFrameworkCore;
using OnlineShop.Library.GoodsService.Models;

namespace OnlineShop.Library.Data
{
    public class ArticlesDbContext : DbContext
    {
        public ArticlesDbContext(DbContextOptions<ArticlesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>()
               .Property(b => b.Id)
               .HasDefaultValueSql("NEWID()");           

            builder.Entity<PriceList>()
              .Property(b => b.Id)
              .HasDefaultValueSql("NEWID()");
            builder.Entity<PriceList>()
            .Property(b => b.Value)
            .HasDefaultValue(0M)
            .HasPrecision(12, 10);

        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<PriceList> PriceLists { get; set; }
    }
}
