using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace auth.Model.ViewModel
{


    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Image { get; set; }

    }
    public class ProductDetailViewModel : ProductViewModel
    {
        public string Color { get; set; }
        public string CaseMeterial { get; set; }
        public float CaseSize { get; set; }
        public string GlassMaterial { get; set; }
        public string Movement { get; set; }
        public int WaterResistant { get; set; }
        public string Description { get; set; }
        public int Warranty { get; set; }
        public string brand { get; set; }
        public string category { get; set; }
    }
}
