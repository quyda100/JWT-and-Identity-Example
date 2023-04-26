using auth.Interfaces;
using auth.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Đăng ký thất bại" });
            }
            return Ok(new { Message = "Đăng ký thành công" });
        }
        [HttpPost]
        [Route("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin(RegisterRequest model)
        {
            var result = await _service.RegisterAdminAsync(model);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Đăng ký thất bại" });
            }
            return Ok(new { Message = "Đăng ký thành công" });
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var result = await _service.LoginAsync(model);
            if (string.IsNullOrEmpty(result))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Đăng nhập thất bại" });
            }
            return Ok(new
            {
                Message = "Đăng nhập thành công",
                Token = result
            });
        }
    }
}
