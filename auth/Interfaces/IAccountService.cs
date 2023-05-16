using auth.Model;
using auth.Model.Request;
using auth.Model.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace auth.Interfaces
{
    public interface IAccountService
    {
        public Task<IdentityResult> RegisterAsync(RegisterRequest model);
        public Task<IdentityResult> RegisterAdminAsync(RegisterRequest model);
        public Task<string> LoginAsync(LoginRequest model);
        public void UpdateProfile(string id, UserDTO model);
        public Task<IdentityResult> ChangePassword(string Id,ChangepasswordRequest model);
        public Task<UserDTO> GetCurrentUser(string id);
        public void ChangeAvatar(string id, IFormFile file);
    }
}
