using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IBrandService
    {
        public List<BrandDTO> GetBrands();
        public List<BrandDTO> GetBrandsTrashed();
        public void AddBrand(BrandRequest model);
        public void UpdateBrand(int id, BrandDTO model);
        public void DeleteBrand(int id);
        public void RecoveryBrand(int id);
    }
}
