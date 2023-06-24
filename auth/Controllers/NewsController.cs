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
    public class NewsController : ControllerBase
    {
        private readonly INewService _service;

        public NewsController(INewService service)
        {
            _service = service;
        }
        [HttpGet("GetNewestPost")]
        [AllowAnonymous]
        public IActionResult GetNewstPost()
        {
            var news = _service.GetNewestPosts();
            return Ok(news);
        }
        [HttpGet("GetPosts")]
        [AllowAnonymous]
        public IActionResult GetPosts()
        {
            var news = _service.GetViewPosts();
            return Ok(news);
        }
        [HttpGet]
        public IActionResult GetNews()
        {
            var news = _service.GetNews();
            return Ok(news);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetNews(int id)
        {
            var news = _service.GetNew(id);
            return Ok(news);
        }
        [HttpPost]
        public IActionResult AddNew([FromForm]NewRequest New)
        {
            try
            {
                _service.AddNew(New);
                return Ok(New);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult UpdateNew(int id, [FromForm] NewUpdateRequest New)
        {
            try
            {
                _service.UpdateNew(id, New);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteNew(int id)
        {
            try
            {
                _service.DeleteNew(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}
