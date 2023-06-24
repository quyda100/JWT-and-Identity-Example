using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using auth.Model.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace auth.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUtilityService _utility;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IConfiguration configuration, ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, IUtilityService utility)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _roleManager = roleManager;
            _utility = utility;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                throw new Exception("Tài khoản hoặc mật khẩu không đúng");
            }

            var roles = await _userManager.GetRolesAsync(user);
            // generate token that is valid for 7 days
            var claims = new List<Claim>
             {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return GenerateJwtToken(claims);
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                throw new Exception("Email already exists!");
            }
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FullName = model.FullName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> RegisterAdminAsync(RegisterRequest model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                throw new Exception("Email already exists!");
            }
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FullName = model.FullName,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            return await _userManager.UpdateAsync(user);
        }
        public string GenerateJwtToken(List<Claim> claims)
        {
            var token_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var creds = new SigningCredentials(token_key, SecurityAlgorithms.HmacSha512Signature);

            var expires = DateTime.Now.AddDays(7);
            var token = new JwtSecurityToken(
               _configuration["Jwt:Issuser"],
              _configuration["Jwt:Audience"],
              claims,
              expires: expires,
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> ChangePassword(ChangepasswordRequest model)
        {
            if (GetUserId() != model.Id)
            {
                throw new Exception("Có lỗi xảy ra");
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                throw new Exception("User is not exist");
            }
            return await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
        }

        public Task<UserDTO> GetCurrentUser()
        {
            var user = GetUserById();
            var userInfo = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Address = user.Address,
                Avatar = user.Avatar,
                Phone = user.PhoneNumber,
                FullName = user.FullName
            };
            return Task.FromResult(userInfo);
        }
        public void UpdateProfile(UserDTO model)
        {
            if (!GetUserId().Equals(model.Id))
            {
                throw new Exception("Không thể lưu thông tin");
            }
            var user = GetUserById();
            if (user.Email != model.Email && _context.Users.Any(u => u.FullName == model.FullName))
            {
                throw new Exception("Email này đã được sử dụng");
            }
            if (!String.IsNullOrEmpty(model.Address))
            {
                user.Address = model.Address;
            }
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            user.Avatar = model.Avatar;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public async void ChangeAvatar(IFormFile file)
        {
            List<string> extensionAllowed = new List<string> { ".png", ".jpg", "jpeg" };
            var user = GetUserById();
            var fileExtension = Path.GetExtension(file.FileName);
            if (!extensionAllowed.Contains(fileExtension))
            {
                throw new Exception("Vui lòng tải lên đúng định dạng");
            }
            var fileName = Path.Combine("Uploads", "Avatar", GetUserId() + fileExtension);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            using (var fs = File.Create(filePath))
            {
                file.CopyTo(fs);
            }
            user.Avatar = fileName;
            await _userManager.UpdateAsync(user);
        }
        private User GetUserById()
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == GetUserId());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task SendResetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Vui lòng nhập Email");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = $"http://localhost:3000/forgotPassword?email={email}&token={HttpUtility.UrlEncode(token)}";
            var content = $"<h3>Chào: {user.FullName}</h3>" +
                $"<p>Bạn vui lòng nhấp <a href ='{link}' target='_blank' style='font-weight: bold;'>VÀO ĐÂY</a> để đặt lại mật khẩu";
            await _utility.SendEmailAsync(user.FullName, email, content);
        }

        public async Task ResetPassword(string email, string token, string newPassword)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                throw new Exception("Vui lòng thử lại");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("Không tìm thấy người dùng");
            }
            var strToken = HttpUtility.UrlDecode(token);
            var result = await _userManager.ResetPasswordAsync(user, strToken, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault().Description);
            }
        }
    }
}
