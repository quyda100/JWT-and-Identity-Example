using auth.Model;

namespace auth.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProducts();
        public Task<IEnumerable<Product>> GetAvailableProducts();
        public Product getProductById(int id);
        public void addProduct(Product product);
        public void removeProduct(int id);
        public void updateProduct(int id, Product product);

        //SimilarProduct
        public Task<List<Product>>  getSimilarProduct(int brandId, int caseSize);

        public Task<List<Product>> getAddCart(string image, string name, int price);

        
    }
}

