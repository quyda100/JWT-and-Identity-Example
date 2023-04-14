using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandsController(IBrandService service)
        {
            _service = service;
        }
        [HttpGet("getBrands")]
        public IActionResult getBrand()
        {
            var brands = _service.getBrands();
            return Ok(new Response
            {
                status = "success",
                data = brands,
                message = "Thành công"
            }
            );
        }
        [HttpPost("addBrand")]
        public IActionResult addBrand(Brand Brand)
        {
            _service.addBrand(Brand);
            return Ok(new Response { status = "success", data = null, message = "Thành công" });
        }
        [HttpPost("updateBrand")]
        public IActionResult updateBrand(int id, Brand Brand)
        {
            _service.updateBrand(id, Brand);
            return Ok(new Response { status = "success", data = null, message = "Thành công" });
        }
        [HttpPost("deleteBrand")]
        public IActionResult deleteBrand(int id)
        {
            _service.deleteBrand(id);
            return Ok(new Response { status = "success", data = null, message = "Thành công" });
        }
    }
}
