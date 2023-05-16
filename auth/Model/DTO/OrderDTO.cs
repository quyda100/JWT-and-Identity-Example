namespace auth.Model.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserFullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long Total { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
        public List<OrderProductDTO> OrderProducts { get; set; }
    }
}
