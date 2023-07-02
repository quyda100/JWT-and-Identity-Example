using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public bool IsDeleted { get; set; } = false;
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
