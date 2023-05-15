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
        [HttpGet("GetCategories")]
        public IActionResult GetCategory()
        {
            var categories = _service.GetCategories();
            return Ok(categories);
        }
        [HttpPost("AddCategory")]
        public IActionResult AddCategory(Category category)
        {
            try
            {
                if (category == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.AddCategory(category);
                _log.saveLog(new Log
                {
                    UserId = getCurrentUserId(),
                    Action = "Tạo mới loại sản phẩm: " + category.Name
                });
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            try
            {
                if (category == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.UpdateCategory(id, category);
                _log.saveLog(new Log
                {
                    UserId = getCurrentUserId(),
                    Action = "Cập nhật loại sản phẩm " + category.Name
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpPost("DeteleCategory")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.DeleteCategory(id);
                _log.saveLog(new Log
                {
                    UserId = getCurrentUserId(),
                    Action = "Xóa loại sản phẩm id: " + id
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
