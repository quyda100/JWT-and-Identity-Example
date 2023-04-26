using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly INewService _service;
        private readonly ILogService _log;

        public NewsController(INewService service, ILogService log)
        {
            _service = service;
            _log = log;
        }
        [HttpGet("getNews")]
        [AllowAnonymous]
        public IActionResult getNews()
        {
            var news = _service.getNews();
            return Ok(new Response
            {
                status = "success",
                data = news,
                message = "Thành công"
            }
            );
        }
        [HttpPost("addNew")]
        [Authorize("")]
        public IActionResult addNew(New New)
        {
            _service.addNew(New);
            return Ok(new Response { status = "success", data = null, message = "Thành công" });
        }
        [HttpPost("updateNew")]
        public IActionResult updateNew(int id, New New)
        {
            _service.updateNew(id, New);
            return Ok(new Response { status = "success", data = null, message = "Thành công" });
        }
        [HttpPost("deleteNew")]
        public IActionResult deleteNew(int id)
        {
            _service.deleteNew(id);
            return Ok(new Response { status = "success", data = null, message = "Thành công" });
        }
    }
}
