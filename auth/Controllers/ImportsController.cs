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
            var result = _service.getImports();
            return Ok(new { status = "success", data = result, message = "Lấy dữ liệu thành công"});
        }
        [HttpGet("{id}")]
        public IActionResult getImportDetail(int importId)
        {
            var result = _service.getImportDetail(importId);
            return Ok(new { status = "success", data = result, message = "Lấy dữ liệu thành công" });
        }
        [HttpPost]
        public IActionResult addImport(ImportRequest request)
        {
            string currenUser = getCurrentUserId();
            _service.addImport(request, currenUser);
            _log.saveLog(new Log { UserId = currenUser, Action = "Nhập sản phẩm" });
            return Ok(new { status = "success", message = "Lưu thành công" });
        }
        private string getCurrentUserId()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return userId;
        }
    }
}
