using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; } = 0;
        public string Image { get; set; }
        public string Color { get; set; }
        public string PreviewImages { get; set; }

        [DisplayName("Chất liệu vỏ")]
        public string CaseMeterial { get; set; }
        [DisplayName("Kích thước mặt")]
        public float CaseSize { get; set; }
        [DisplayName("Chất liệu kính")]
        public string GlassMaterial { get; set; }
        [DisplayName("Loại máy")]
        public string Movement { get; set; }
        [DisplayName("Kháng nước")]
        public int WaterResistant { get; set; }
        public string Description { get; set; }
        [DisplayName("Bảo hành")]
        public int Warranty { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        [ForeignKey("Brand")]
        public int BrandId { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        #region
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        #endregion
    }
}
