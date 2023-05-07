using auth.Interfaces;
using auth.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _service.RegisterAsync(model);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đăng ký thất bại" });
            }
            return Ok(new { message = "Đăng ký thành công" });
        }
        [HttpPost]
        [Route("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin(RegisterRequest model)
        {
            var result = await _service.RegisterAdminAsync(model);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đăng ký thất bại" });
            }
            return Ok(new { message = "Đăng ký thành công" });
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var result = await _service.LoginAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đăng nhập thất bại" });
            }
            return Ok(new
            {
                message = "Đăng nhập thành công",
                token = result
            });
        }
        [HttpPost("Changepassword")]
        public async Task<IActionResult> ChangePassword (ChangepasswordRequest model)
        {
            var result = await _service.ChangePassword(model);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đổi mật khẩu thất bại", data = result.Errors });
            }
            return Ok(new
            {
                message = "Đổi mật khẩu thành công"
            });
        }
    }
}
