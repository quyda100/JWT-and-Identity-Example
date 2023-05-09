using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllProducts()
        {
           return Ok(_service.GetProducts());
        }
        [HttpGet("GetAvailableProducts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableProducts()
        {
            try
            {
                return Ok(await _service.GetAvailableProducts());
            }
            catch
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetProductById(int id)
        {
            var product = _service.getProductById(id);
            return Ok(product);
        }
        [HttpPost("AddProduct")]
        public IActionResult Add(Product product)
        {
            try
            {
                if(product == null || !ModelState.IsValid) {
                    return BadRequest("Lỗi: Data rỗng");
                }
                var isExist = _service.checkExist(product.Id);
                if (isExist)
                {
                    return BadRequest(product.Id + " đã tồn tại!");
                }
                _service.addProduct(product);
            }
            catch (Exception)
            {

                return BadRequest("Không thể tạo sản phẩm");
            }
            return Ok("Đã tạo thành công: "+ product.Name);
           
        }
        [HttpPut]
        public IActionResult Update(Product product)
        {
            try
            {
                if (product == null || !ModelState.IsValid)
                {
                    return BadRequest("Lỗi: Data rỗng");
                }
                var isExist = _service.checkExist(product.Id);
                if (!isExist)
                {
                    return NotFound(product.Name + " không tồn tại!");
                }
                _service.updateProduct(product);
            }
            catch (Exception)
            {

                return BadRequest("Không thể sửa sản phẩm");
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var isExist = _service.checkExist(id);
                if (!isExist)
                {
                    return NotFound(id + " không tồn tại!");
                }
                _service.removeProduct(id);
            }
            catch (Exception)
            {

                return BadRequest("Không thể xóa sản phẩm");
            }
            return NoContent();
        }

        [HttpGet("SimilarProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSimilarProduct(int brandId, int caseSize)
        {
            var product = await _service.getSimilarProduct(brandId, caseSize);

            return Ok(product);
        }
        [HttpGet("GetProductsByBrand")]
        [AllowAnonymous]
        public IActionResult GetProductsByBrand(int brandId)
        {
            var products = _service.getProductsByBrand(brandId);
            return Ok(new { status = "success", data = products, message = "Lấy sản phẩm thành công" });
        }
        [HttpGet("GetProductsByCategory")]
        [AllowAnonymous]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _service.getProductsByCategory(categoryId);
            return Ok(new { status = "success", data = products, message = "Lấy sản phẩm thành công" });
        }
        [HttpGet("GetNewestProduct")]
        [AllowAnonymous]
        public IActionResult GetNewstProduct(int category)
        {
            var products = _service.getNewestProducts(category);
            return Ok(new { status = "success", data = products, message = "Lấy sản phẩm thành công" });
        }
    }
}