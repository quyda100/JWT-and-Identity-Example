using auth.Data;
using auth.Interfaces;
using auth.Model;
using AutoMapper;
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
            var products = await _context.Products.ToListAsync();
            return products;
        }
        public async void addProduct(Product p)
        {
            if (await _context.Products.AnyAsync(p => p.Name == p.Name))
                throw new Exception(p.Name +" is exist");
            _context.Products.Add(p);
            await _context.SaveChangesAsync();

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
    }
}
