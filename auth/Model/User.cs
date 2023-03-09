using Microsoft.AspNetCore.Identity;

namespace auth.Model
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
    }
}
