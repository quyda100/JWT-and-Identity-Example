using auth.Model;
using auth.Model.Request;
using auth.Model.DTO;

namespace auth.Interfaces
{
    public interface IProductService
    {
        public List<ProductDTO> GetProducts();
        public List<ProductDTO> GetAvailableProducts();
        public ProductDTO GetProductById(int id);
        public void AddProduct(ProductCreateRequest product);
        public void RemoveProduct(int id);
        public void UpdateProduct(int id, ProductRequest product);

        //SimilarProduct
        public List<Product> GetSimilarProduct(int brandId, int caseSize);
        public List<Product> GetProductsByBrand(int brandId);
        public List<Product> GetProductsByCategory(int categoryId);
        public List<Product> GetFeatureProduct();
        public List<Product> GetNewestProducts(int categoryId);
    }
}

