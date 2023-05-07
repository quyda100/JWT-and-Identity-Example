using System.ComponentModel.DataAnnotations;

namespace auth.Model.Request
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class RegisterRequest
    {

        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ComfirmPassword { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
    }
    public class ChangepasswordRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string newpassword { get; set; }
    }
}
