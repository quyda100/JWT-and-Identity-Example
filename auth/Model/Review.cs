using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public Product Product { get; set; }
    }
}
