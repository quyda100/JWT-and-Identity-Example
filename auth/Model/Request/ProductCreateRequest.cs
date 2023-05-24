namespace auth.Model.Request
{
    public class ProductCreateRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Color { get; set; }
        public List<IFormFile> PreviewImageFiles { get; set; }
        public string CaseMaterial { get; set; }
        public float CaseSize { get; set; }
        public string GlassMaterial { get; set; }
        public string Movement { get; set; }
        public int WaterResistant { get; set; }
        public string Description { get; set; }
        public int Warranty { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}
