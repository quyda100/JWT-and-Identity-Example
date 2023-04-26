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

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet("getCategories")]
        public IActionResult getCategory() {
            var categories = _service.getCategories();
            return Ok(new
            {
                status = "success",
                data = categories,
                message = "Thành công"
            }
            );
        }
        [HttpPost("addCategory")]
        public IActionResult addCategory(Category category)
        {
            _service.addCategory(category);
            return Ok(new
            {
                status = "success",
                message = "Thành công"
            }
            );
        }
        [HttpPost("updateCategory")]
        public IActionResult updateCategory(int id, Category category) { 
            _service.updateCategory(id, category);
            return Ok(new
            {
                status = "success",
                message = "Thành công"
            }
            );
        }
        [HttpPost("deteleCategory")]
        public IActionResult deleteCategory(int id) {
            _service.deleteCategory(id);
            return Ok(new
            {
                status = "success",
                message = "Thành công"
            }
            );
        }

    }
}
