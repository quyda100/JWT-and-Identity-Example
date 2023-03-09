namespace auth.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public float Total { get; set; }
        public bool IsApproved { get; set; } = false;
        public string PaymentMethod { get; set; }
        public DateTime PaymentTime { get; set; }
        public int OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
