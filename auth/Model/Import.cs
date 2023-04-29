using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Model
{
    public class Import
    {
        public int Id { get; set; }
        public int Total { get; set; }
        [ForeignKey("User")]
        public string UserId {get; set; }
        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public List<ImportDetail> Details { get; set; }
        #region
        public User User { get; set; }
        public Supplier Supplier { get; set; }
        #endregion

    }
}
