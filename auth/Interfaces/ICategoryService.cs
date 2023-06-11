using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface ICategoryService
    {
        public List<CategoryDTO> GetCategories();
        public void AddCategory(CategoryRequest model);
        public void UpdateCategory(int id, CategoryDTO model);
        public void DeleteCategory(int id);
    }
}
