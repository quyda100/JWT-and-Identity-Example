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
        public  void addProduct(Product product)
        {
            try
            {
                if (_context.Products.Any(x => x.Name == product.Name))
                    throw new Exception(product.Name + " is exist");
                _context.Products.Add(product);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {

            }

        }
            
        //public void addProduct(Product model)
        //{
        //    if (_context.Products.Any(x => x.Name == model.Name))
        //        throw new Exception("Name '" + model.Name + "' Product is already");
        //    var product = new Product()
        //    {
        //        Code = model.Code,
        //        Name = model.Name,
        //        Price = model.Price,
        //        Stock = model.Stock,
        //        Image = model.Image,
        //        Color = model.Color,
        //        CaseMeterial = model.CaseMeterial,
        //        CaseSize = model.CaseSize,
        //        GlassMaterial = model.GlassMaterial,
        //        Movement = model.Movement,
        //        WaterResistant = model.WaterResistant,
        //        Description = model.Description,
        //        Warranty = model.Warranty,
        //        IsDeleted= model.IsDeleted,
        //        BrandId= model.BrandId,
        //        Gender = model.Gender,
        //    };
        //    _context.Products.Add(product);
        //    _context.SaveChanges();
        //}

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



        // api  SimilarProduct
        public async Task<List<Product>> getSimilarProduct(int brandId, int caseSize)
        {
            var products = _context.Products.Where(p => p.BrandId == brandId && p.CaseSize==caseSize).ToList();
            if(products == null)
            {
                throw new Exception("Product not found");
            }
            return products;
        }
    }
}
