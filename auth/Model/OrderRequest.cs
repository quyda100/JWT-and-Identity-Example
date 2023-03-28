using System.ComponentModel.DataAnnotations;

namespace auth.Model
{
    public class OrderRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public List<OrderProductRequest> orderProducts { get; set; }
    }
    public class OrderProductRequest
    {
        public int ProductId { get; set; }
        public int Quanlity { get; set; }
        public int Price { get; set; }
    }
}
