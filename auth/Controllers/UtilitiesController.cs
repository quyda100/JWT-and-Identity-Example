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

        [HttpGet("images/products/{name}")]
        public IActionResult GetProductImage(string name) {
            name = Path.Combine("products", name);
            return File(_service.GetImage(name), "image/jpeg");
        }
    }
}
