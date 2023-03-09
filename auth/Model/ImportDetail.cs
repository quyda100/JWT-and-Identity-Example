using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class ImportDetail
    {
        public int Id { get; set; }
        [ForeignKey("Import")]
        public int ImportId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public float Price { get; set; }
        public int Quanlity { get; set; }
    }
}
