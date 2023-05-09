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
        [HttpGet("getBrands")]
        public IActionResult getBrand()
        {
            var brands = _service.getBrands();
            return Ok(new
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
            _log.saveLog(new Log{
                UserId = getCurrentUserId(),
                Action = "Created new brand: " + Brand.Name
            });
            return Ok(new { status = "success", message = "Tạo brand thành công" });
        }
        [HttpPost("updateBrand")]
        public IActionResult updateBrand(int id, Brand Brand)
        {
            _service.updateBrand(id, Brand);
            _log.saveLog(new Log{
                UserId = getCurrentUserId(),
                Action = "Updated brand: " + Brand.Name
            });
            return Ok(new { status = "success", message = "Cập nhật thành công" });
        }
        [HttpPost("deleteBrand")]
        public IActionResult deleteBrand(int id)
        {
            _service.deleteBrand(id);
            _log.saveLog(new Log{
                UserId = getCurrentUserId(),
                Action = "Deleted brandId: " + id
            });
            return Ok(new { status = "success", message = "Xóa thành công" });
        }
        private string getCurrentUserId(){
            var userId = HttpContext.User.Claims.FirstOrDefault(c=>c.Type == "UserId").Value;
            return userId;
        }
    }
}
