using auth.Model;

namespace auth.Interfaces
{
    public interface IBrandService
    {
        public List<Brand> GetBrands();
        public void AddBrand(Brand model);
        public void UpdateBrand(int id, Brand model);
        public void DeleteBrand(int id);
    }
}
