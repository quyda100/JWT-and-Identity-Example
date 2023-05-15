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
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _service.GetProductById(id);
                return Ok(product);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddProduct")]
        public IActionResult Add(ProductRequest product)
        {
            try
            {
                if(!ModelState.IsValid || product == null)
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
        public IActionResult Update(int id, ProductRequest product)
        {
            try
            {
                if (!ModelState.IsValid || product == null)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.UpdateProduct(id, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
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

        [HttpGet("SimilarProduct")]
        [AllowAnonymous]
        public  IActionResult GetSimilarProduct(int brandId, int caseSize)
        {
            var product =  _service.GetSimilarProduct(brandId, caseSize);
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
    }
}