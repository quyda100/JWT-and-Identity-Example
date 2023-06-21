using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace auth.Services
{
    public class ImportService : IImportService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _log;
        private readonly IMapper _mapper;

        public ImportService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogService log, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
            _mapper = mapper;
        }
        public async Task AddImport(ImportRequest model)
        {
            var import = new Import
            {
                UserId = GetUserId(),
                SupplierId = model.supplierId,
            };
            _context.Imports.Add(import);
            await _context.SaveChangesAsync();
            var total = 0;
            model.ImportProducts.ForEach(item =>
            {
                var product = GetProductByCode(item.ProductCode);
                /*
                 *  Create Import Product
                 */
                var importProduct = new ImportDetail
                {
                    ImportId = import.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    Price = item.Price,
                };
                total += importProduct.Quantity * importProduct.Price;
                _context.ImportDetails.Add(importProduct);
                /*
                 *  Update Product
                 */
                product.Stock += importProduct.Quantity;
                product.Price = importProduct.Price;
                product.UpdatedAt = DateTime.Now;
                _context.Products.Update(product);
            });
            import.Total = total;
            _log.SaveLog("Tạo hóa đơn nhập: " + import.Id);
            _context.Imports.Update(import);
            await _context.SaveChangesAsync();
        }
        public async Task ImportByCSV(ImportFileRequest request)
        {
            var fileExtension = Path.GetExtension(request.file.FileName);
            if (!fileExtension.Equals(".csv"))
            {
                throw new Exception("You must upload .csv file");
            }
            /*
            *   Create path of file
            */
            var fileName = DateTime.Now.ToString() + fileExtension;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "CSV");
            var filePath = Path.Combine(folderPath, fileName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (var stream = File.Create(filePath))
            {
                request.file.CopyTo(stream);
            }
            /*
            *   Reading file
            */
            var items = new List<ImportProductRequest>();
            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<ImportProductRequest>().ToList();
                    items.AddRange(records);
                }
            }
            /*
            *   Call back to AddImport
            */
            var ImportRequest = new ImportRequest { supplierId = request.supplierId, ImportProducts = items };
            await AddImport(ImportRequest);
        }
        private Product GetProductByCode(string code)
        {
            var product = _context.Products.FirstOrDefault(p => p.Code == code);
            if (product == null)
                throw new Exception("Không tìm thấy sản phẩm");
            return product;
        }
        public List<ImportProductDTO> GetImportDetail(int id)
        {
            var importDetails = _context.ImportDetails.Where(i => i.ImportId == id).Include(i => i.Product).Select(i => _mapper.Map<ImportProductDTO>(i)).ToList();
            return importDetails;
        }

        public List<ImportDTO> GetImports()
        {
            var imports = _context.Imports.Include(i => i.User).Include(i=>i.Supplier).Select(i => _mapper.Map<ImportDTO>(i)).ToList();
            return imports;
        }
        private string GetUserId()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
