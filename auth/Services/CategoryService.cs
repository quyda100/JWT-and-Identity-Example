using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;

        public CategoryService(ApplicationDBContext context, ILogService log)
        {
            _context = context;
            _log = log;
        }
        public void AddCategory(CategoryRequest model)
        {

            if (_context.Categories.Any(x => x.Name == model.Name))
                throw new Exception(model.Name + " đã tồn tại!");
            _log.SaveLog("Tạo mới loại sản phẩm: " + model.Name);
            var cate = new Category
            {
                Name = model.Name,
                Description = model.Description,
                Type = model.Type,
            };
            _context.Categories.Add(cate);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategory(id);
            category.IsDeleted = true;
            _log.SaveLog("Xóa loại sản phẩm: " + category.Name);
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public List<CategoryDTO> GetCategories()
        {
            var categories = _context.Categories.Where(c => c.IsDeleted == false).Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Type = c.Type,
            }).ToList();
            return categories;
        }

        public List<CategoryDTO> GetCategoriesTrashed()
        {
            var categories = _context.Categories.Where(c => c.IsDeleted == true).Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }).ToList();
            return categories;
        }

        public void RecoveryCategory(int id)
        {
            var category = GetCategory(id);
            category.IsDeleted = false;
            _log.SaveLog("Khôi phục loại sản phẩm: " + category.Name);
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(int id, CategoryDTO model)
        {
            if (model.Id != id)
                throw new Exception("Có lỗi xảy ra");
            var category = GetCategory(id);
            if (model.Name != category.Name && _context.Products.Any(pr => pr.Name == model.Name))
                throw new Exception(category.Name + " đã tồn tại");
            category.Name = model.Name;
            category.Description = model.Description;
            category.Type = model.Type;
            category.UpdatedAt = DateTime.UtcNow.AddHours(7);
            _log.SaveLog("Cập nhật dữ liệu: " + category.Name);
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        private Category GetCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (category == null)
                throw new Exception("Loại sản phẩm không tồn tại");
            return category;
        }
    }
}
