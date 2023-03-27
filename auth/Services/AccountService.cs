using auth.Data;
using auth.Helpers;
using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<User> userManager,RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, IConfiguration configuration, ApplicationDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<string> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if( user!= null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                // generate token that is valid for 7 days
                var claims = new List<Claim>
             {
               new Claim("Name", user.FullName),
               new Claim("Email", user.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                return GenerateJwtToken(claims);
            }

            return String.Empty;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if(userExists != null)
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
                throw new Exception("User creation failed!");
            }

            //if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            //{
            //    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            //}
            //if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            //{
            //    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            //}
            return await _userManager.UpdateAsync(user);
        }

        public User GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if(user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
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
    }
}
