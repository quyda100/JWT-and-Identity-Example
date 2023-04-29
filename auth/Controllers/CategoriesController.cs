using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly ILogService _log;
        public CategoriesController(ICategoryService service, ILogService log)
        {   
            _service = service;
            _log = log;
        }
        [AllowAnonymous]
        [HttpGet("getCategories")]
        public IActionResult getCategory() {
            var categories = _service.getCategories();
            return Ok(new
            {
                status = "success",
                data = categories,
                message = "Lấy dữ liệu thành công"
            }
            );
        }
        [HttpPost("addCategory")]
        public IActionResult addCategory(Category category)
        {
            _service.addCategory(category);
            _log.saveLog(new Log{
                UserId = getCurrentUserId(),
                Action = "Created category: " + category.Name
            });
            return Ok(new
            {
                status = "success",
                message = "Thêm category thành công"
            }
            );
        }
        [HttpPost("updateCategory")]
        public IActionResult updateCategory(int id, Category category) { 
            _service.updateCategory(id, category);
            _log.saveLog(new Log{
                UserId = getCurrentUserId(),
                Action = "Updated category: " + category.Name
            });
            return Ok(new
            {
                status = "success",
                message = "Cập nhật thành công"
            }
            );
        }
        [HttpPost("deteleCategory")]
        public IActionResult deleteCategory(int id) {
            _service.deleteCategory(id);
            _log.saveLog(new Log{
                UserId = getCurrentUserId(),
                Action = "Deleted category id: " + id
            });
            return Ok(new
            {
                status = "success",
                message = "Xóa thành công"
            }
            );
        }
                private string getCurrentUserId(){
            var userId = HttpContext.User.Claims.FirstOrDefault(c=>c.Type == "UserId").Value;
            return userId;
        }
    }
}
