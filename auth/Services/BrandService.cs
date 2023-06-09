using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;

        public BrandService(ApplicationDBContext context, ILogService log)
        {
            _context = context;
            _log = log;
        }

        public List<BrandDTO> GetBrands()
        {
            var brands = _context.Brands.Select(b => new BrandDTO
            {
                Id = b.Id,
                Name = b.Name,
                CreatedAt = b.CreatedAt
            }).ToList();
            return brands;
        }

        public void AddBrand(string name)
        {
            if (_context.Brands.Any(x => x.Name == name))
                throw new Exception(name + " đã tồn tại!");
            _log.SaveLog("Tạo nhãn hàng mới: " + name);
            var brand = new Brand
            {
                Name = name
            };
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }

        public void DeleteBrand(int id)
        {
            var brand = GetBrand(id);
            brand.IsDeleted = true;
            _log.SaveLog("Xóa nhãn hàng: " + brand.Name);
            _context.Brands.Update(brand);
            _context.SaveChanges();
        }

        public void UpdateBrand(int id, BrandDTO model)
        {
            if (model.Id != id)
                throw new Exception("Có lỗi xảy ra");
            var brand = GetBrand(id);
            if (model.Name != brand.Name && _context.Products.Any(pr => pr.Name == model.Name))
                throw new Exception("Tên " + brand.Name + " đã tồn tại");
            brand.Name = model.Name;
            brand.UpdatedAt = DateTime.Now;
            _log.SaveLog("Cập nhật dữ liệu: " + brand.Name);
            _context.Brands.Update(brand);
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
