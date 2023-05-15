using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDBContext _context;

        public BrandService(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Brand> getBrands()
        {
            var brands = _context.Brands.ToList();
            return brands;
        }

        public void AddBrand (Brand model)
        {
            if (_context.Brands.Any(x => x.Name == model.Name))
                throw new Exception(model.Name + " is exist");
            _context.Brands.Add(model);
            _context.SaveChanges();
        }

        public void DeleteBrand(int id)
        {
            var brand = GetBrand(id);
            brand.IsDeleted = true;
            _context.Brands.Update(brand);
            _context.SaveChanges();
        }

        public void UpdateBrand(int id, Brand model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var brand = GetBrand(id);
            if (model.Name != brand.Name && _context.Products.Any(pr => pr.Name == model.Name))
                throw new Exception("Name " + brand.Name + " is already taken");
            model.UpdatedAt = DateTime.Now;
            _context.Brands.Update(model);
            _context.SaveChangesAsync();
        }

        private Brand GetBrand(int id)
        {
            var brand = _context.Brands.SingleOrDefault(x => x.Id == id);
            if (brand == null)
                throw new Exception("Brand not found");
            return brand;
        }
    }
}
