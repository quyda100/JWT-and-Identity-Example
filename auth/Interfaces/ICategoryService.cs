using auth.Model;
using auth.Model.DTO;

namespace auth.Interfaces
{
    public interface ICategoryService
    {
        public List<CategoryDTO> GetCategories();
        public void AddCategory(string Name);
        public void UpdateCategory(int id, CategoryDTO model);
        public void DeleteCategory(int id);
    }
}
