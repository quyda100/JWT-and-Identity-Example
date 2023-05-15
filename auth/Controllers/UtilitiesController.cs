using auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilityService _service;

        public UtilitiesController(IUtilityService service)
        {
            _service = service;
        }

        [HttpGet("images/{name}")]
        public IActionResult GetImage(string name) {
            return File(_service.GetImage(name), "image/jpeg");
        }
    }
}
