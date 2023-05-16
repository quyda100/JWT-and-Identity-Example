using System.ComponentModel.DataAnnotations;

namespace auth.Model.ViewModel
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        [RegularExpression("^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$")]
        public string Phone { get; set; }
    }
}
