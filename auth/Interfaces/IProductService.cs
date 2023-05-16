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
        public ProductDetailViewModel GetProductByCode(string code);
        public void AddProduct(ProductRequest product);
        public void RemoveProduct(int id);
        public void UpdateProduct(int id, ProductRequest product);

        //SimilarProduct
        public List<Product> GetSimilarProduct(int brandId);
        public List<Product> GetProductsByBrand(int brandId);
        public List<Product> GetProductsByCategory(int categoryId);
        public List<Product> GetFeatureProduct();
        public List<Product> GetNewestProducts(int categoryId);
    }
}

