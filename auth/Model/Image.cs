using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class Image
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string Path { get; set; }
        public Product Product { get; set; }
    }
}
