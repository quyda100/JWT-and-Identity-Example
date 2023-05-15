using auth.Model;

namespace auth.Interfaces
{
    public interface ICategoryService
    {
        public List<Category> GetCategories();
        public void AddCategory(Category model);
        public void UpdateCategory(int id, Category model);
        public void DeleteCategory(int id);
    }
}
