using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;
        private readonly ILogService _log;

        public BrandsController(IBrandService service, ILogService log)
        {
            _service = service;
            _log = log;
        }
        [AllowAnonymous]
        [HttpGet("GetBrands")]
        public IActionResult GetBrand()
        {
            var brands = _service.GetBrands();
            return Ok(brands);
        }
        [HttpPost]
        public IActionResult AddBrand(Brand brand)
        {
            try
            {
                if (brand == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.AddBrand(brand);
                _log.saveLog(new Log
                {
                    UserId = getCurrentUserId(),
                    Action = "Tạo mới nhãn hàng: " + brand.Name
                });
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateBrand(int id, Brand brand)
        {
            try
            {
                if (brand == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.UpdateBrand(id, brand);
                _log.saveLog(new Log
                {
                    UserId = getCurrentUserId(),
                    Action = "Cập nhật nhãn hàng: " + brand.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteBrand(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.DeleteBrand(id);
                _log.saveLog(new Log
                {
                    UserId = getCurrentUserId(),
                    Action = "Xóa nhãn hàng Id: " + id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        private string getCurrentUserId()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            if (userId == null)
            {
                throw new Exception("Vui lòng đăng nhập lại");
            }
            return userId;
        }
    }
}
