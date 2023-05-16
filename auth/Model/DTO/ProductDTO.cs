using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace auth.Model.ViewModel
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public string CaseMeterial { get; set; }
        public float CaseSize { get; set; }
        public string GlassMaterial { get; set; }
        public string Movement { get; set; }
        public int WaterResistant { get; set; }
        public string Description { get; set; }
        public int Warranty { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public string PreviewImages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
    }
}
