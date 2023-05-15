using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;

        public CategoryService(ApplicationDBContext context, ILogService log) { 
            _context = context;
            _log = log;
        }
        public void AddCategory(Category model)
        {

            if (_context.Categories.Any(x => x.Name == model.Name))
                throw new Exception(model.Name + " đã tồn tại!");
            _log.SaveLog("Tạo mới loại sản phẩm: " + model.Name);
            _context.Categories.Add(model);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = GetCategory(id);
            category.IsDeleted = true;
            _log.SaveLog("Xóa loại sản phẩm: "+category.Name);
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public List<Category> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }

        public void UpdateCategory(int id, Category model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var category = GetCategory(id);
            if (model.Name != category.Name && _context.Products.Any(pr => pr.Name == model.Name))
                throw new Exception(category.Name + " đã tồn tại");
            model.UpdatedAt = DateTime.Now;
            _log.SaveLog("Cập nhật dữ liệu: " + category.Name);
            _context.Categories.Update(model);
            _context.SaveChangesAsync();
        }
        private Category GetCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (category == null)
                throw new Exception("Category not found");
            return category;
        }
    }
}
