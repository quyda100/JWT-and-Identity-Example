using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDBContext _context;

        public BrandService(ApplicationDBContext context) {
            _context = context;
        }

        public async Task<IEnumerable<Brand>> getBrands()
        {
            var brands = await _context.Brands.ToListAsync();
            return brands;
        }

        void IBrandService.addBrand(Brand model)
        {
            try
            {
                if (_context.Brands.Any(x => x.Name == model.Name))
                    throw new Exception(model.Name + " is exist");
                _context.Brands.Add(model);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        void IBrandService.deleteBrand(int id)
        {
            var brand = GetBrand(id);
            brand.IsDeleted = true;
            _context.Brands.Update(brand);
            _context.SaveChanges();
        }

        void IBrandService.updateBrand(int id, Brand model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var brand = GetBrand(id);
            if (model.Name != brand.Name && _context.Products.Any(pr => pr.Name == model.Name))
                throw new Exception("Name " + brand.Name + " is already taken");
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
