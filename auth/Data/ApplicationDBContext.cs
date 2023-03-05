using auth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace auth.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        #region DBSet
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion

        #region Seeding
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                Name = "Đồng hồ A",
                Price = 200, Stock = 10, IsDeleted = false
            });
            builder.Entity<Product>().HasData(new Product
            {
                Id = 2,
                Name = "Đồng hồ B",
                Price = 300,
                Stock = 1,
                IsDeleted = false
            });
            builder.Entity<Product>().HasData(new Product
            {
                Id = 3,
                Name = "Đồng hồ C",
                Price = 800,
                Stock = 0,
                IsDeleted = true
            });
        }
        #endregion
    }
}
