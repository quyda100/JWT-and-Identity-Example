using System.ComponentModel.DataAnnotations;

namespace auth.Model
{
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
        
        public string Avatar { get; set; } 
    }
}
