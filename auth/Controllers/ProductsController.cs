using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                return Ok(await _service.GetProducts());
            }
            catch
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _service.getProductById(id);
            return Ok(product);
        }
        [HttpPost("AddProduct")]
        [Authorize]
        public IActionResult Add(Product product)
        {
            _service.addProduct(product);
            return Ok(new {message = "Thêm sản phẩm thành công"});
        }
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, Product product)
        {
            _service.updateProduct(id, product);
            return Ok(new { message = "Cập nhật sản phẩm thành công" });
        }
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            _service.removeProduct(id);
            return Ok(new { message = "Xóa sản phẩm thành công" });
        }

        [HttpGet("SimilarProduct")]
        public async Task<IActionResult> GetSimilarProduct(int brandId, int caseSize)
        {
            var product = await _service.getSimilarProduct(brandId, caseSize);

            return Ok(product);
        }
    }
}
