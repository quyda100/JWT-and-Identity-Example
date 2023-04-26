using Microsoft.AspNetCore.Identity;

namespace auth.Model
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; } = "static.israel21c.org/www/uploads/2018/07/israel-sunset-ashkelon-september.jpg";
        public List<Log> Logs {get;set;}
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}

