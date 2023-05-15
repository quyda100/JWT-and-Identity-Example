namespace auth.Model.Request
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public List<IFormFile> PreviewImageFiles { get; set; }
        public string PreviewImages { get; set; }
        public string CaseMeterial { get; set; }
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
