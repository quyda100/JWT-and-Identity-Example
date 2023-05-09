using auth.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace auth.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        #region DBSet
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<ImportDetail> ImportDetails { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Log> Logs {get;set;}

        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.LogTo(Console.WriteLine);

        #region Seeding
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach(var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<Import>().ToTable("Imports");
            builder.Entity<ImportDetail>().ToTable("ImportDetails");
            builder.Entity<Review>().ToTable("Reviews");
            builder.Entity<Supplier>().ToTable("Suppilers");
            builder.Entity<New>().ToTable("News");
            builder.Entity<Log>().ToTable("Logs");
        }
        #endregion

    }
}
