﻿using System.ComponentModel.DataAnnotations;

namespace auth.Model.Request
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
