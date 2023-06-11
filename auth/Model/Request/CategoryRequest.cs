using System.ComponentModel.DataAnnotations;

namespace auth.Model.Request
{
    public class CategoryRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
