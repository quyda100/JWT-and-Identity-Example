using Microsoft.Extensions.Options;
using auth.Helpers;
using auth.Interfaces;

namespace auth.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IAccountService accountService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var email = jwtUtils.ValidateJwtToken(token);
            if (email != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = accountService.GetUserByEmail(email);
            }

            await _next(context);
        }
    }
}
