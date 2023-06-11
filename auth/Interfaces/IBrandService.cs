using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IBrandService
    {
        public List<BrandDTO> GetBrands();
        public void AddBrand(BrandRequest model);
        public void UpdateBrand(int id, BrandDTO model);
        public void DeleteBrand(int id);
    }
}
