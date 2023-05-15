using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using auth.Model.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace auth.Services
{
    public class ProductService : IProductService
    {
        private readonly IUtilityService _utility;
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;

        public ProductService(ApplicationDBContext context, IUtilityService utility, ILogService log)
        {
            _utility = utility;
            _context = context;
            _log = log;
        }
        public List<ProductViewModel> GetProducts()
        {
            var products = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            var productsDTO = products.Select(p => new ProductViewModel { Id = p.Id, Code = p.Code, Name = p.Name, Price = p.Price, Image = p.Image }).ToList();
            return productsDTO;
        }
        public List<ProductDetailViewModel> GetProductsDetail()
        {
            var products = _context.Products.Include(p => p.Brand).Include(p => p.Category);
            var productDeTailDTO = products.Select(p => new ProductDetailViewModel
            {
                Id = p.Id,
                Code = p.Code,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image,
                Color = p.Color,
                CaseMeterial = p.CaseMeterial,
                CaseSize = p.CaseSize,
                GlassMaterial = p.GlassMaterial,
                Movement = p.Movement,
                WaterResistant = p.WaterResistant,
                Description = p.Description,
                Warranty = p.Warranty,
                BrandName = p.Brand.Name,
                CatetoryName = p.Category.Name,
                PreviewImages = p.PreviewImages,
            }).ToList();
            return productDeTailDTO;
        }

        public List<ProductViewModel> GetAvailableProducts()
        {
            var products = _context.Products.Where(product => product.Stock > 0).Include(p => p.Brand).Include(p => p.Category).ToList();
            var productsDTO = products.Select(p => new ProductViewModel { Id = p.Id, Code = p.Code, Name = p.Name, Price = p.Price, Image = p.Image }).ToList();
            return productsDTO;
        }
        public void AddProduct(ProductRequest productDTO, string userId)
        {
            if (_context.Products.Any(x => x.Name == productDTO.Name))
            {
                throw new Exception(productDTO.Name + " đã tồn tại!");
            }
            if(_context.Products.Any(p=> p.Code == productDTO.Code))
            {
                throw new Exception(productDTO.Code + " đã tồn tại");
            }
            var image = _utility.UploadImage(productDTO.ImageFile, productDTO.Code);
            var lstPreviewImages = _utility.UploadImages(productDTO.PreviewImageFiles, productDTO.Code);
            var previewImages = lstPreviewImages == null ? null : JsonSerializer.Serialize(lstPreviewImages);
            var product = new Product
            {
                Code = productDTO.Code,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Image = image,
                Color = productDTO.Color,
                PreviewImages = previewImages,
                CaseMeterial = productDTO.CaseMeterial,
                CaseSize = productDTO.CaseSize,
                GlassMaterial = productDTO.GlassMaterial,
                Movement = productDTO.Movement,
                WaterResistant = productDTO.WaterResistant,
                Description = productDTO.Description,
                Warranty = productDTO.Warranty,
                BrandId = productDTO.BrandId,
                CategoryId = productDTO.CategoryId,
            };
            _log.saveLog(new Log { UserId = userId, Action = "Tạo mới sản phẩm: " + product.Code });
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void RemoveProduct(int id, string userId)
        {
            var product = GetProduct(id);
            product.IsDeleted = true;
            _log.saveLog(new Log { UserId = userId, Action = "Xóa sản phẩm: " + product.Code });
            _context.Products.Update(product);
            _context.SaveChangesAsync();
        }

        public void UpdateProduct(int id, ProductRequest p, string userId)
        {
            if (p.Id != id)
                throw new Exception("Having trouble");
            var product = GetProduct(id);
            if (product.Name != p.Name && _context.Products.Any(pr => pr.Name == p.Name))
                throw new Exception("Name " + p.Name + " is already taken");
            var productUpdate = new Product
            {
                Code = p.Code,
                Name = p.Name,
                Price = p.Price,
                Image = p.Image,
                Color = p.Color,
                PreviewImages = p.PreviewImages,
                CaseMeterial = p.CaseMeterial,
                CaseSize = p.CaseSize,
                GlassMaterial = p.GlassMaterial,
                Movement = p.Movement,
                WaterResistant = p.WaterResistant,
                Description = p.Description,
                Warranty = p.Warranty,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
            };
            _log.saveLog(new Log { UserId = userId, Action = "Cập nhật sản phẩm: " + product.Code });
            _context.Products.Update(productUpdate);
            _context.SaveChangesAsync();
        }
        public ProductDetailViewModel GetProductById(int id)
        {
            var product = GetProduct(id);
            var productDTO = new ProductDetailViewModel
            {
                Id = id,
                Code = product.Code,
                Name = product.Name,
                Price = product.Price,
                Image = product.Image,
                Color = product.Color,
                CaseMeterial = product.CaseMeterial,
                CaseSize = product.CaseSize,
                GlassMaterial = product.GlassMaterial,
                Movement = product.Movement,
                WaterResistant = product.WaterResistant,
                Description = product.Description,
                Warranty = product.Warranty,
                BrandName = product.Brand.Name,
                CatetoryName = product.Category.Name,
                PreviewImages = product.PreviewImages,
            };
            return productDTO;
        }

        private Product GetProduct(int id)
        {
            var product = _context.Products.Include(p => p.Brand).Include(p => p.Category).SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            return product;
        }
        public List<Product> GetSimilarProduct(int brandId, int caseSize)
        {
            var products = _context.Products.Where(p => p.BrandId == brandId && p.CaseSize == caseSize).ToList();
            if (products == null)
            {
                throw new Exception("Product not found");
            }
            return products;
        }

        public Product SearchProduct(string name)
        {
            var result = _context.Products.FirstOrDefault(n => n.Name == name);
            if (result == null)
            {
                throw new Exception("Product not found");
            }
            return result;
        }

        public List<Product> GetProductsByBrand(int brandId)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.Id == brandId);
            if (brand == null)
            {
                throw new Exception("Brand is not exist!");
            }
            var products = _context.Products.Where(p => p.BrandId == brandId).Include(p => p.Brand).Include(p => p.Category).ToList();
            return products;
        }
        public List<Product> GetProductsByCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(b => b.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category is not exist!");
            }
            var products = _context.Products.Where(p => p.CategoryId == categoryId).Include(p => p.Brand).Include(p => p.Category).ToList();
            return products;
        }

        public List<Product> GetFeatureProduct()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetNewestProducts(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(b => b.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category is not exist!");
            }
            var products = _context.Products.Where(p => p.CategoryId == categoryId).OrderBy(p => p.CreatedAt).Take(4).Include(p => p.Brand).Include(p => p.Category).ToList();
            return products;
        }
    }
}
