using auth.Model;

namespace auth.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> getCategories();
        public void addCategory(Category model);
        public void updateCategory(int id, Category model);
        public void deleteCategory(int id);
    }
}
