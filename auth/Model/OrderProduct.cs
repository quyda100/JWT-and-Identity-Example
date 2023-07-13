using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class OrderProduct
    {
        public int Id { get; set; }
        [ForeignKey("Oder")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public float DiscountPrice { get; set; }
    }
}
