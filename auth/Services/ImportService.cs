using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using System.Security.Claims;

namespace auth.Services
{
    public class ImportService : IImportService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _log;

        ImportService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogService log)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
        }
        public void AddImport(ImportRequest model)
        {
            var import = new Import
            {
                UserId = GetUserId(),
                SupplierId = model.supplierId,
            };
            _context.Imports.Add(import);
            _context.SaveChanges();
            var total = 0;
            model.ImportProducts.ForEach(Iproduct =>
            {
                /*
                 *  Create Import Product
                 */
                var importProduct = new ImportDetail
                {
                    ProductId = Iproduct.ProductId,
                    Quanlity = Iproduct.Quanlity,
                    Price = Iproduct.Price,
                };
                total += importProduct.Quanlity * importProduct.Price;
                _context.ImportDetails.Add(importProduct);
                /*
                 *  Update Product
                 */
                var product = GetProductById(importProduct.Id);
                product.Stock += importProduct.Quanlity;
                product.Price = importProduct.Price;
                product.UpdatedAt = DateTime.Now;
                _context.Products.Update(product);
            });
            import.Total = total;
            _log.SaveLog("Nhập sản phẩm: "+import.Id);
            _context.Imports.Update(import);
            _context.SaveChanges();
        }
        private Product GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new Exception("Product not found");
            return product;
        }
        public List<ImportDetail> GetImportDetail(int id)
        {
            var importDetails = _context.ImportDetails.Where(i => i.ImportId == id).ToList();
            return importDetails;
        }

        public List<Import> GetImports()
        {
            var imports = _context.Imports.ToList();
            return imports;
        }
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
