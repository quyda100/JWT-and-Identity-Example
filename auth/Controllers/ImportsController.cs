using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class ImportsController : ControllerBase
    {
        private readonly IImportService _service;
        private readonly ILogService _log;

        public ImportsController(IImportService service, ILogService log)
        {
            _service = service;
            _log = log;
        }
        [HttpGet]
        public IActionResult getImports()
        {
            var result = _service.GetImports();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetImportDetail(int importId)
        {
            var result = _service.GetImportDetail(importId);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddImport(ImportRequest request)
        {
            try
            {
                _service.AddImport(request);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
