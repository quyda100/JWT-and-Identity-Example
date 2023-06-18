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
        public ImportsController(IImportService service)
        {
            _service = service;
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
        public async Task<IActionResult> AddImport(ImportRequest request)
        {
            try
            {
                await _service.AddImport(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ImportFromFile")]
        public async Task<IActionResult> ImportByCSV([FromForm]ImportFileRequest request){
            try
            {
                if(!ModelState.IsValid || request == null){
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                await _service.ImportByCSV(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}
