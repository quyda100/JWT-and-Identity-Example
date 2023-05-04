namespace auth.Model.Request
{
    public class ImportRequest
    {
        public int supplierId { get; set; }
        public List<ImportProductRequest> ImportProducts { get; set; }
    }
    public class ImportProductRequest
    {
        public int ProductId { get; set; }
        public int Price { get; set; }
        public int Quanlity { get; set; }
    }
}
