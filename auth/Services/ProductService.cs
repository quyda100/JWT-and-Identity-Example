using auth.Data;
using auth.Interfaces;
using auth.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;
        public ProductService(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = await _context.Products.Include(p=>p.Brand).Include(p=>p.Category).ToListAsync();
            return products;
        }
        public async Task<IEnumerable<Product>> GetAvailableProducts()
        {
            var products = await _context.Products.Where(product => product.Stock > 0).Include(p=>p.Brand).Include(p=>p.Category).ToListAsync();
            return products;
        }
        public void addProduct(Product product)
        {
                if (_context.Products.Any(x => x.Name == product.Name))
                    throw new Exception(product.Name + " is exist");
                _context.Products.Add(product);
                _context.SaveChanges();

        }
        public void removeProduct(int id)
        {
            var product = getProduct(id);
            product.IsDeleted = true;
            _context.Products.Update(product);
            _context.SaveChangesAsync();
        }

        public void updateProduct(int id, Product p)
        {
            if (p.Id != id)
                throw new Exception("Having trouble");
            var product = getProduct(id);
            if (product.Name != p.Name && _context.Products.Any(pr => pr.Name == p.Name))
                throw new Exception("Name " + p.Name + " is already taken");
            p.UpdatedAt = DateTime.Now;
            _context.Products.Update(p);
            _context.SaveChangesAsync();
        }
        public Product getProductById(int id)
        {
            return getProduct(id);
        }

        private Product getProduct(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            return product;
        }
        public Task<List<Product>> getSimilarProduct(int brandId, int caseSize)
        {
            var products = _context.Products.Where(p => p.BrandId == brandId && p.CaseSize==caseSize).ToList();
            if(products == null)
            {
                throw new Exception("Product not found");
            }
            return Task.FromResult(products);
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

        public List<Product> getProductsByBrand(int brandId)
        {
            var brand = _context.Brands.FirstOrDefault(b=>b.Id==brandId);
            if (brand == null)
            {
                throw new Exception("Brand is not exist!");
            }
            var products = _context.Products.Where(p => p.BrandId == brandId).Include(p => p.Brand).Include(p => p.Category).ToList();
            return products;
        }
        public List<Product> getProductsByCategory(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(b => b.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category is not exist!");
            }
            var products = _context.Products.Where(p => p.CategoryId == categoryId).Include(p => p.Brand).Include(p => p.Category).ToList();
            return products;
        }

        public List<Product> getFeatureProduct()
        {
            throw new NotImplementedException();
        }

        public List<Product> getNewestProducts(int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(b => b.Id == categoryId);
            if (category == null)
            {
                throw new Exception("Category is not exist!");
            }
            var products = _context.Products.Where(p=>p.CategoryId == categoryId).OrderBy(p=>p.CreatedAt).Take(4).Include(p => p.Brand).Include(p => p.Category).ToList();
            return products;
        }
        }
}
