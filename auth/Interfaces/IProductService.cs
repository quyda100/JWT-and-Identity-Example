using auth.Model;

namespace auth.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        /*public bool addProduct(Product product);
        public bool removeProduct(int id);
        public bool updateProduct(int id, Product product);*/
    }
}
