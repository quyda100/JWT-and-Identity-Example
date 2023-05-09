using auth.Model;

namespace auth.Interfaces
{
    public interface IBrandService
    {
        public Task<IEnumerable<Brand>> getBrands();
        public void addBrand(Brand model);
        public void updateBrand(int id, Brand model);
        public void deleteBrand(int id);
    }
}
