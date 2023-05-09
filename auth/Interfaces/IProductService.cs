using auth.Model;

namespace auth.Interfaces
{
    public interface IProductService
    {
        public bool checkExist(int id);
        public Task<IEnumerable<Product>> GetProducts();
        public Task<IEnumerable<Product>> GetAvailableProducts();
        public Product getProductById(int id);
        public void addProduct(Product product);
        public void removeProduct(int id);
        public void updateProduct(Product product);

        //SimilarProduct
        public Task<List<Product>> getSimilarProduct(int brandId, int caseSize);
        public List<Product> getProductsByBrand(int brandId);
        public List<Product> getProductsByCategory(int categoryId);
        public List<Product> getFeatureProduct();
        public List<Product> getNewestProducts(int categoryId);
    }
}

