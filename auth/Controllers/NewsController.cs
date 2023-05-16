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
    public class NewsController : ControllerBase
    {
        private readonly INewService _service;

        public NewsController(INewService service)
        {
            _service = service;
        }
        [HttpGet("getNews")]
        [AllowAnonymous]
        public IActionResult GetNews()
        {
            var news = _service.GetNews();
            return Ok(news);
        }
        [HttpPost("addNew")]
        public IActionResult AddNew(New New)
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
        [HttpPost("updateNew")]
        public IActionResult UpdateNew(int id, New New)
        {
            try
            {
                _service.UpdateNew(id, New);
            }
            catch (Exception)
            {

                throw;
            }
            return NoContent();
        }
        [HttpPost("deleteNew")]
        public IActionResult DeleteNew(int id)
        {
            try
            {
                _service.DeleteNew(id);
            }
            catch (Exception)
            {

                throw;
            }
            return NoContent();
        }
    }
}
