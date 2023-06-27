﻿using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using auth.Model.DTO;
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
        private readonly IMapper _mapper;

        public ProductService(ApplicationDBContext context, IUtilityService utility, ILogService log, IMapper mapper)
        {
            _utility = utility;
            _context = context;
            _log = log;
            _mapper = mapper;
        }
        public List<ProductDTO> GetProducts()
        {
            var products = _context.Products.Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }
        public List<ProductDTO> GetProductsDetail()
        {
            var products = _context.Products.Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }

        public List<ProductDTO> GetAvailableProducts()
        {
            var products = _context.Products.Where(product => product.Stock > 0).Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }
        public void AddProduct(ProductRequest productDTO)
        {
            if (_context.Products.Any(x => x.Name == productDTO.Name))
            {
                throw new Exception(productDTO.Name + " đã tồn tại!");
            }
            if (_context.Products.Any(p => p.Code == productDTO.Code))
            {
                throw new Exception($"'{productDTO.Code}' đã tồn tại");
            }
            var image = _utility.UploadImage(productDTO.ImageFile, productDTO.Code, "Products");
            var lstPreviewImages = new List<string>();
            lstPreviewImages.AddRange(_utility.UploadImages(productDTO.PreviewImageFiles, productDTO.Code, "Products"));
            var previewImages = lstPreviewImages == null ? null : JsonSerializer.Serialize(lstPreviewImages);
            var product = new Product
            {
                Code = productDTO.Code,
                Name = productDTO.Name,
                Price = productDTO.Price,
                Image = image,
                Color = productDTO.Color,
                PreviewImages = previewImages,
                CaseMaterial = productDTO.CaseMaterial,
                CaseSize = productDTO.CaseSize,
                GlassMaterial = productDTO.GlassMaterial,
                Movement = productDTO.Movement,
                WaterResistant = productDTO.WaterResistant,
                Description = productDTO.Description,
                Warranty = productDTO.Warranty,
                BrandId = productDTO.BrandId,
                CategoryId = productDTO.CategoryId,
            };
            _log.SaveLog("Tạo mới sản phẩm: " + product.Code);
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void RemoveProduct(int id)
        {
            var product = GetProduct(id);
            if (product.Stock != 0)
            {
                throw new Exception("Sản phẩm còn hàng");
            }
            product.IsDeleted = true;
            _log.SaveLog("Xóa sản phẩm: " + product.Code);
            _context.Products.Update(product);
            _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(int id, ProductRequest p)
        {
            if (p.Id != id)
                throw new Exception("Có lỗi xảy ra");
            var product = GetProduct(id);
            if (product.Code != p.Code && _context.Products.Any(pr => pr.Code == p.Code))
                throw new Exception(p.Code + " đã tồn tại");
            if (product.Name != p.Name && _context.Products.Any(pr => pr.Name == p.Name))
                throw new Exception(p.Name + " đã tồn tại");
            var image = _utility.UploadImage(p.ImageFile, p.Code, "Products");
            var previewImages = _utility.UploadImages(p.PreviewImageFiles, p.Code, "Products");
            if (!String.IsNullOrEmpty(image))
            {
                product.Image = image;
            }
            if (previewImages != null)
            {
                var images = JsonSerializer.Deserialize<List<string>>(product.PreviewImages);
                var deleted = images.Where(i => !p.PreviewImages.Contains(i)).ToList();
                deleted.ForEach(i => images.Remove(i));
                var combine = images.Concat(previewImages);
                product.PreviewImages = JsonSerializer.Serialize(combine);
            }
            product.Code = p.Code;
            product.Name = p.Name;
            product.Price = p.Price;
            product.Color = p.Color;
            product.CaseMaterial = p.CaseMaterial;
            product.CaseSize = p.CaseSize;
            product.GlassMaterial = p.GlassMaterial;
            product.Movement = p.Movement;
            product.WaterResistant = p.WaterResistant;
            product.Description = p.Description;
            product.Warranty = p.Warranty;
            product.BrandId = p.BrandId;
            product.CategoryId = p.CategoryId;
            product.UpdatedAt = DateTime.Now;
            _log.SaveLog("Cập nhật sản phẩm: " + product.Code);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public ProductDTO GetProductById(int id)
        {
            var product = GetProduct(id);
            return _mapper.Map<ProductDTO>(product);
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
        public List<ProductDTO> GetSimilarProduct(string brandName)
        {
            var products = _context.Products.Where(p => p.Brand.Name == brandName).Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            if (products == null)
            {
                throw new Exception("Product not found");
            }
            return products;
        }

        public ProductDTO SearchProduct(string name)
        {
            var result = _context.Products.Where(p => p.Name.Contains(name)).ToList();
            if (result == null)
            {
                throw new Exception("Product not found");
            }
            return _mapper.Map<ProductDTO>(result);
        }

        public List<ProductDTO> GetProductsByBrand(int brandId)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.Id == brandId);
            if (brand == null)
            {
                //throw new Exception("Brand is not exist!");
                return null;
            }
            var products = _context.Products.Where(p => p.BrandId == brandId).Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }
        public List<ProductDTO> GetProductsByCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(b => b.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category is not exist!");
            }
            var products = _context.Products.Where(p => p.CategoryId == categoryId).Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }

        public List<ProductDTO> GetFeatureProduct()
        {
            throw new NotImplementedException();
        }

        public List<ProductDTO> GetNewestProducts(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(b => b.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category is not exist!");
            }
            var products = _context.Products.Where(p => p.CategoryId == categoryId).OrderByDescending(p => p.CreatedAt).Take(4).Include(p => p.Brand).Include(p => p.Category).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }

        public List<ProductDTO> GetTrashedProducts()
        {
            var products = _context.Products.Where(p => p.IsDeleted == true).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }

        public ProductDTO GetProductByCode(string code)
        {
            var product = _context.Products.Include(p => p.Brand).Include(p => p.Category).FirstOrDefault(p => p.Code == code);
            return _mapper.Map<ProductDTO>(product);
        }

        public void RecoveryProduct(int id)
        {
            var product = GetProduct(id);
            product.IsDeleted = false;
            _context.Products.Update(product);
            _log.SaveLog("Khôi phục sản phẩm: " + product.Name);
            _context.SaveChanges();
        }
    }
}
