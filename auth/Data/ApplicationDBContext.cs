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
            /*builder.Entity<Brand>().HasData(new Brand
            {
                Name = "LONGINES",
                Description = ""
            });
            builder.Entity<Brand>().HasData(new Brand
            {
                Name = "TISSOT",
                Description = ""
            });
            builder.Entity<Brand>().HasData(new Brand
            {
                Name = "MIDO",
                Description = ""
            });
            builder.Entity<Brand>().HasData(new Brand
            {
                Name = "CITIZEN",
                Description = ""
            });
            builder.Entity<Brand>().HasData(new Brand
            {
                Name = "CERTINA",
                Description = ""
            });

            builder.Entity<Category>().HasData(new Category
            {
                Name = "Đồng hồ nam",
                Description = ""
            }); builder.Entity<Category>().HasData(new Category
            {
                Name = "Đồng hồ nữ",
                Description = ""
            }); builder.Entity<Category>().HasData(new Category
            {
                Name = "Đồng hồ đôi",
                Description = ""
            });

            builder.Entity<Product>().HasData(new Product
            {
                Name = "LONGINES PRESENCE",
                Code = "L3.781.3.06.7",
                Price = 149500000,
                Stock = 10,
                Image = "",
                CaseSize = (float)38.5,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Đen",
                WaterResistant = 30,
                Warranty = 5,
                BrandId = 1,
                CategoryId = 1,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "LONGINES HYDROCONQUEST",
                Code = "L3.781.3.06.7",
                Price = 47868750,
                Stock = 10,
                Image = "",
                CaseSize = 41,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ PVD & gốm",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Đen",
                WaterResistant = 300,
                Warranty = 5,
                BrandId = 1,
                CategoryId = 1,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "LONGINES PRÉSENCE",
                Code= "L4.322.4.92.2",
                Price = 60077250,
                Stock = 10,
                Image = "",
                CaseSize = 40,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Xanh lam",
                WaterResistant = 300,
                Warranty = 5,
                BrandId = 1,
                CategoryId = 3,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "LONGINES MASTER",
                Code= "L2.793.5.57.7",
                Price = 103500000,
                Stock = 10,
                Image = "",
                CaseSize = 40,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L/ Vàng 18K",
                Movement = "Automatic (Máy cơ tự động)",
                Color= "Đen",
                WaterResistant = 30,
                Warranty = 5,
                BrandId = 1,
                CategoryId = 2,
                Description = ""
            });
            //-----------------------------------------//
            builder.Entity<Product>().HasData(new Product
            {
                Name = "TISSOT EVERYTIME MEDIUM",
                Code = "T109.410.22.033.00",
                Price = 8667000,
                Stock = 10,
                Image = "",
                CaseSize = 38,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Quartz (Máy pin - điện tử)",
                Color= "Trắng",
                WaterResistant = 30,
                Warranty = 2,
                BrandId = 2,
                CategoryId = 1,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "TISSOT LOVELY ROUND",
                Code = "T140.009.11.111.00",
                Price = 103500000,
                Stock = 10,
                Image = "",
                CaseSize = 22,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Quartz (Máy pin - điện tử)",
                Color = "Trắng",
                WaterResistant = 30,
                Warranty = 2,
                BrandId = 2,
                CategoryId = 2,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "TISSOT LOVELY ROUND",
                Code = "T140.009.16.111.00",
                Price = 7560000,
                Stock = 10,
                Image = "",
                CaseSize = 20,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Quartz (Máy pin - điện tử)",
                Color = "Trắng",
                WaterResistant = 30,
                Warranty = 2,
                BrandId = 2,
                CategoryId = 2,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "TISSOT PRX",
                Code = "T137.410.33.021.00",
                Price = 25200000,
                Stock = 10,
                Image = "",
                CaseSize = 35,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Quartz (Máy pin - điện tử)",
                Color = "Vàng",
                WaterResistant = 100,
                Warranty = 5,
                BrandId = 2,
                CategoryId = 3,
                Description = ""
            });
            //-----------------------------------------//
            builder.Entity<Product>().HasData(new Product
            {
                Name = "MIDO MULTIFORT POWERWIND",
                Code = "M040.408.11.041.00",
                Price = 35000000,
                Stock = 10,
                Image = "",
                CaseSize = 40,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Automatic Chronometer (Máy cơ tự động chuẩn COSC)",
                Color = "Xanh lam",
                WaterResistant = 50,
                Warranty = 2,
                BrandId = 3,
                CategoryId = 1,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "MIDO BARONCELLI LADY NECKLACE",
                Code = "M037.807.22.031.00",
                Price = 32155500,
                Stock = 10,
                Image = "",
                CaseSize = 33,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Khảm trai",
                WaterResistant = 50,
                Warranty = 2,
                BrandId = 3,
                CategoryId = 2,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "MIDO BARONCELLI MIDNIGHT BLUE",
                Code = "M7600.3.65.8",
                Price = 47817000,
                Stock = 10,
                Image = "",
                CaseSize = 38,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Xanh lam",
                WaterResistant = 50,
                Warranty = 2,
                BrandId = 3,
                CategoryId = 3,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "MIDO BARONCELLI II SIGNATURE",
                Code = "M037.407.11.041.00",
                Price = 43344000,
                Stock = 10,
                Image = "",
                CaseSize = 39,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Xanh lam",
                WaterResistant = 50,
                Warranty = 2,
                BrandId = 3,
                CategoryId = 3,
                Description = ""
            });

            //-----------------------------------------//
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CITIZEN BI5030-51E",
                Code = "BI5030-51E",
                Price = 6264500,
                Stock = 10,
                Image = "",
                CaseSize = 39,
                GlassMaterial = "Mặt kính cứng",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Quartz (Máy pin - điện tử)",
                Color = "Đen",
                WaterResistant = 30,
                Warranty = 1,
                BrandId = 4,
                CategoryId = 3,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CITIZEN NY4053-05A",
                Code = "NY4053-05A",
                Price = 14722000,
                Stock = 10,
                Image = "",
                CaseSize = 40,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Đen",
                WaterResistant = 50,
                Warranty = 1,
                BrandId = 4,
                CategoryId = 3,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CITIZEN ECO-DRIVE FE1140-51X",
                Code = "FE1140-51X",
                Price = 4998000,
                Stock = 10,
                Image = "",
                CaseSize = 29,
                GlassMaterial = "Mặt kính cứng",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Eco-Drive (Năng lượng ánh sáng)",
                Color = "Hồng",
                WaterResistant = 50,
                Warranty = 1,
                BrandId = 4,
                CategoryId = 2,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CITIZEN BI5100-58A",
                Code = "BI5100-58A",
                Price = 3472250,
                Stock = 10,
                Image = "",
                CaseSize = 40,
                GlassMaterial = "Mặt kính cứng",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Quartz (Máy pin - điện tử)",
                Color = "Trắng",
                WaterResistant = 50,
                Warranty = 1,
                BrandId = 4,
                CategoryId = 1,
                Description = ""
            });
            //-----------------------------------------//
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CERTINA DS PODIUM CHRONOMETER ",
                Code = "C034.654.36.057.00",
                Price = 18486000,
                Stock = 10,
                Image = "",
                CaseSize = 42,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ công nghệ PVD",
                Movement = "Quartz Chronometer (Máy pin chuẩn COSC)",
                Color = "Đen",
                WaterResistant = 100,
                Warranty = 2,
                BrandId = 5,
                CategoryId = 1,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CERTINA DS-8 MOON PHASE",
                Code = "C033.457.16.031.00",
                Price = 12132000,
                Stock = 10,
                Image = "",
                CaseSize = 41,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ 316L",
                Movement = "Quartz Chronometer (Máy pin chuẩn COSC)",
                Color = "Đen",
                WaterResistant = 100,
                Warranty = 3,
                BrandId = 5,
                CategoryId = 1,
                Description = ""
            });
            builder.Entity<Product>().HasData(new Product
            {
                Name = "CERTINA DS CAIMANO LADY POWERMATIC",
                Code = "C035.207.22.037.02",
                Price = 15631500,
                Stock = 10,
                Image = "",
                CaseSize = 31,
                GlassMaterial = "Sapphire",
                CaseMeterial = "Thép không gỉ mạ vàng công nghệ PVD",
                Movement = "Automatic (Máy cơ tự động)",
                Color = "Trắng",
                WaterResistant = 100,
                Warranty = 3,
                BrandId = 5,
                CategoryId = 2,
                Description = ""
            });*/
        }
        #endregion
    }
}
