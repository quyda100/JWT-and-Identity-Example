using auth.Data;
using auth.Helpers;
using auth.Model.Request;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth.Authorization
{
    public interface IJwtUtils
        {
            public string GenerateJwtToken(LoginRequest model);
            public string ValidateJwtToken(string token);
        }

        public class JwtUtils : IJwtUtils
        {
            private ApplicationDBContext _context;
            private readonly AppSettings _appSettings;

            public JwtUtils(
                ApplicationDBContext context,
                IOptions<AppSettings> appSettings)
            {
                _context = context;
                _appSettings = appSettings.Value;
            }

            public string GenerateJwtToken(LoginRequest model)
            {
                // generate token that is valid for 15 minutes
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("Email", model.Email) }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }

            public string ValidateJwtToken(string token)
            {
                if (token == null)
                    return null;

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                var Email = jwtToken.Claims.First(x => x.Type == "Email").Value;

                    // return user id from JWT token if validation successful
                    return Email;
                }
                catch
                {
                    // return null if validation fails
                    return null;
                }
            }
        }
    }
