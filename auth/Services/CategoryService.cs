using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDBContext _context;

        public CategoryService(ApplicationDBContext context) { _context = context; }
        public void addCategory(Category model)
        {
            try
            {
                if (_context.Categories.Any(x => x.Name == model.Name))
                    throw new Exception(model.Name + " is exist");
                _context.Categories.Add(model);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void deleteCategory(int id)
        {
            var category = getCategory(id);
            category.IsDeleted = true;
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Category>> getCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public void updateCategory(int id, Category model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            var category = getCategory(id);
            if (model.Name != category.Name && _context.Products.Any(pr => pr.Name == model.Name))
                throw new Exception("Name " + category.Name + " is already taken");
            _context.Categories.Update(model);
            _context.SaveChangesAsync();
        }
        private Category getCategory(int id)
        {
            var category = _context.Categories.SingleOrDefault(x => x.Id == id);
            if (category == null)
                throw new Exception("Category not found");
            return category;
        }
    }
}
