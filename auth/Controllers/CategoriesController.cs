using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
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
        public IActionResult AddCategory(CategoryRequest model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.AddCategory(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory(int id, CategoryDTO category)
        {
            try
            {
                if (category == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.UpdateCategory(id, category);
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
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpGet("GetTrashed")]
        public IActionResult GetBrandTrashed()
        {
            try
            {
                var categories = _service.GetCategoriesTrashed();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Recovery")]
        public IActionResult Recovery(int id)
        {
            try
            {
                _service.RecoveryCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
