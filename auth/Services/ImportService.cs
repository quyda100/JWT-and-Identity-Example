using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;

namespace auth.Services
{
    public class ImportService : IImportService
    {
        private readonly ApplicationDBContext _context;

        ImportService(ApplicationDBContext context)
        {
            _context = context;
        }
        public void addImport(ImportRequest model)
        {
            var import = new Import
            {
                UserId = model.UserId,
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
                var product = getProductById(importProduct.Id);
                product.Stock += importProduct.Quanlity;
                product.Price = importProduct.Price;
                _context.Products.Update(product);
            });
            import.Total = total;
            _context.Imports.Update(import);
            _context.SaveChanges();
        }
        private Product getProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                throw new Exception("Product not found");
            return product;
        }
        public List<ImportDetail> getImportDetail(int id)
        {
            var importDetails = _context.ImportDetails.Where(i => i.ImportId == id).ToList();
            return importDetails;
        }

        public List<Import> getImports()
        {
            var imports = _context.Imports.ToList();
            return imports;
        }
    }
}
