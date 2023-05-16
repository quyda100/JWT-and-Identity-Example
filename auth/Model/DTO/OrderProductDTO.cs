namespace auth.Model.DTO
{
    public class OrderProductDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductImage { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

    }
}
