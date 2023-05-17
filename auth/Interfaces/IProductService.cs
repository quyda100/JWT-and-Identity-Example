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
        public ProductDTO GetProductByCode(string code);

        //SimilarProduct
        public List<ProductDTO> GetSimilarProduct(string brandName);
        public List<ProductDTO> GetProductsByBrand(int brandId);
        public List<ProductDTO> GetProductsByCategory(int categoryId);
        public List<ProductDTO> GetFeatureProduct();
        public List<ProductDTO> GetNewestProducts(int categoryId);
        public List<ProductDTO> GetTrashedProducts();
    }
}

