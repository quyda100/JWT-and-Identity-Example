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
    public class LogsController : ControllerBase
    {
        private readonly ILogService _service;

        public LogsController(ILogService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetLogs());
        }
    }
}
