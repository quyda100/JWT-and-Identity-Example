namespace auth.Model.DTO
{
    public class ImportProductDTO
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
    }
}