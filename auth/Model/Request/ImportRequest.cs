namespace auth.Model.Request
{
    public class ImportRequest
    {
        public int supplierId { get; set; }
        public List<ImportProductRequest> ImportProducts { get; set; }
    }
    public class ImportProductRequest
    {
        public string ProductCode { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
    public class ImportFileRequest
    {
        public int supplierId {get; set;}
        public IFormFile file {get;set;}
    }
}
