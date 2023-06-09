using auth.Model;
using auth.Model.DTO;

namespace auth.Interfaces
{
    public interface IBrandService
    {
        public List<BrandDTO> GetBrands();
        public void AddBrand(string name);
        public void UpdateBrand(int id, BrandDTO model);
        public void DeleteBrand(int id);
    }
}
