using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace auth.Model
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; } = "https://png.pngtree.com/png-vector/20220709/ourmid/pngtree-businessman-user-avatar-wearing-suit-with-red-tie-png-image_5809521.png";
        public List<Log> Logs {get;set;}
        public List<Review> Reviews { get;set;}
        public List<Order> Orders { get;set;}
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}

