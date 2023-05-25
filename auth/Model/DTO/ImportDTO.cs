namespace auth.Model.DTO
{
    public class ImportDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SupplierName { get; set; }
        public long Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}