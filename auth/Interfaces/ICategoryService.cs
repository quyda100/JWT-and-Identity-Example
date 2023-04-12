using auth.Model;

namespace auth.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<Category>> getCategories();
        public void addCategory();
        public void updateCategory();
        public void deleteCategory();
    }
}
