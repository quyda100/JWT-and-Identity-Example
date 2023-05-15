using auth.Model;
using auth.Model.Request;
using auth.Model.ViewModel;

namespace auth.Interfaces
{
    public interface IProductService
    {
        public List<ProductViewModel> GetProducts();
        public List<ProductDetailViewModel> GetProductsDetail();
        public List<ProductViewModel> GetAvailableProducts();
        public ProductDetailViewModel GetProductById(int id);
        public void AddProduct(ProductRequest product, string userId);
        public void RemoveProduct(int id, string userId);
        public void UpdateProduct(int id, ProductRequest product, string userId);

        //SimilarProduct
        public List<Product> GetSimilarProduct(int brandId, int caseSize);
        public List<Product> GetProductsByBrand(int brandId);
        public List<Product> GetProductsByCategory(int categoryId);
        public List<Product> GetFeatureProduct();
        public List<Product> GetNewestProducts(int categoryId);
    }
}

