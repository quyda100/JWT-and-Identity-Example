using auth.Model;

namespace auth.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Product getProductById(int id);
        public void addProduct(Product product);
        public void removeProduct(int id);
        public void updateProduct(int id, Product product);
    }
}
