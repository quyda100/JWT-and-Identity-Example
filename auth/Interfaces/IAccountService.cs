﻿using auth.Model;
using Microsoft.AspNetCore.Identity;

namespace auth.Interfaces
{
    public interface IAccountService
    {
        public Task<IdentityResult> RegisterAsync(RegisterRequest model);
        public Task<string> LoginAsync(LoginRequest model);
        public User GetUserByEmail(string email);
    }
}
