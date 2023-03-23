namespace auth.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Total { get; set; }
        public int Status { get; set; } 
        public string PaymentMethod { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<OrderProduct> OrderProducts { get; set; }

    }
}
