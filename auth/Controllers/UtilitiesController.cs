using auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("/")]
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilityService _service;

        public UtilitiesController(IUtilityService service)
        {
            _service = service;
        }

        [HttpGet("images/{name}")]
        public IActionResult GetProductImage(string name)
        {
            name = name.Replace("%2F",@"\").ToLower();
            return File(_service.GetImage(name), "image/jpeg");
        }
    }
}
