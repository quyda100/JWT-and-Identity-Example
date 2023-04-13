using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewService _service;

        public NewsController(INewService service)
        {
            _service = service;
        }
        [HttpGet("getNews")]
        public IActionResult getNews()
        {
            return Ok(_service.getNews());
        }
        [HttpPost("addNew")]
        public IActionResult addNew(New New)
        {
            _service.addNew(New);
            return Ok(new { message = "Thành công" });
        }
        [HttpPost("updateNew")]
        public IActionResult updateNew(int id, New New)
        {
            _service.updateNew(id, New);
            return Ok(new { message = "Thành công" });
        }
        public IActionResult deleteNew(int id)
        {
            _service.deleteNew(id);
            return Ok(new { message = "Thành công" });
        }
    }
}
