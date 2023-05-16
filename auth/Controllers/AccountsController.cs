using auth.Interfaces;
using auth.Model.Request;
using auth.Model.ViewModel;
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
            try
            {
                var result = await _service.RegisterAsync(model);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, result.Errors);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin(RegisterRequest model)
        {
            try
            {
                var result = await _service.RegisterAdminAsync(model);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đăng ký thất bại" });
                }
                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        
        public async Task<IActionResult> Login(LoginRequest model)
        {
            try
            {
                var result = await _service.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Changepassword")]
        public async Task<IActionResult> ChangePassword (ChangepasswordRequest model)
        {
            try
            {
                var userId = getCurrentUserId();
                if (userId == null)
                {
                    return BadRequest("Bạn chưa đăng nhập");
                }
                var result = await _service.ChangePassword(userId,model);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpGet("GetCurrentUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userInfo = await _service.GetCurrentUser(getCurrentUserId());
            return Ok(userInfo);
        }
        [HttpPost("UpdateUserInfo")]
        public IActionResult UpdateUserInfo(UserDTO model)
        {
            try
            {
                if(model == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng dữ liệu");
                }
                var userId = getCurrentUserId();
                if(userId == null)
                {
                    return BadRequest("Bạn chưa đăng nhập");
                }
                _service.UpdateProfile(userId, model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [HttpPost("UpdateAvatar")]
        public IActionResult UpdateAvatar(IFormFile file)
        {
            try
            {
                if (file == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng tải lên file");
                }
                var userId = getCurrentUserId();
                if (userId == null)
                {
                    return BadRequest("Bạn chưa đăng nhập");
                }
                _service.ChangeAvatar(userId, file);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        private string getCurrentUserId()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return userEmail;
        }
    }
}
