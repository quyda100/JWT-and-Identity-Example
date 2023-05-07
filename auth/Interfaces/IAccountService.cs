using auth.Model;
using auth.Model.Request;
using Microsoft.AspNetCore.Identity;

namespace auth.Interfaces
{
    public interface IAccountService
    {
        public Task<IdentityResult> RegisterAsync(RegisterRequest model);
        public Task<IdentityResult> RegisterAdminAsync(RegisterRequest model);
        public Task<string> LoginAsync(LoginRequest model);
        public Task<IdentityResult> ChangePassword(ChangepasswordRequest model)
        public User GetUserByEmail(string email);
    }
}
