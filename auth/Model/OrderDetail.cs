using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        [ForeignKey("Oder")]
        public int OrderId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quanlity { get; set; }
        public float Price { get; set; }

        #region
        public Product Product { get; set; }
        public Order Order { get; set; }
        #endregion
    }
}
