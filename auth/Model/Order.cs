namespace auth.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Total { get; set; }
        public int Status { get; set; } = 0;
        public string PaymentMethod { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public User User { get; set; }

    }
}
