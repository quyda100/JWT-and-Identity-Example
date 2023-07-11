using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
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
        public IActionResult GetAvailableProducts()
        {
            return Ok(_service.GetAvailableProducts());
        }
        [HttpGet("{code}")]
        [AllowAnonymous]
        public IActionResult GetProductByCode(string code)
        {
            try
            {
                var product = _service.GetProductByCode(code);
                return Ok(product);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProduct")]
        [Consumes("multipart/form-data")]
        public IActionResult Add([FromForm] ProductRequest product)
        {
            try
            {
                if (!ModelState.IsValid || product == null)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.AddProduct(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductRequest product)
        {
            try
            {
                if (!ModelState.IsValid || product == null)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                await _service.UpdateProduct(id, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.RemoveProduct(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpGet("GetTrashedProducts")]
        public IActionResult GetTrashedProducts()
        {
            return Ok(_service.GetTrashedProducts());
        }

        [HttpGet("SimilarProduct/{brandName}")]
        [AllowAnonymous]
        public IActionResult GetSimilarProduct(string brandName)
        {
            var product = _service.GetSimilarProduct(brandName);
            return Ok(product);
        }
        [HttpGet("GetProductsByBrand")]
        [AllowAnonymous]
        public IActionResult GetProductsByBrand(int brandId)
        {
            var products = _service.GetProductsByBrand(brandId);
            return Ok(products);
        }
        [HttpGet("GetProductsByCategory")]
        [AllowAnonymous]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _service.GetProductsByCategory(categoryId);
            return Ok(products);
        }
        [HttpGet("GetNewestProduct")]
        [AllowAnonymous]
        public IActionResult GetNewstProduct(int category)
        {
            var products = _service.GetNewestProducts(category);
            return Ok(products);
        }
        [HttpPut("Recovery")]
        public IActionResult Recovery(int id)
        {
            try
            {
                _service.RecoveryProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("SearchProduct")]
        public IActionResult SearchProduct(string name)
        {
            try
            {
                var products = _service.SearchProduct(name);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}