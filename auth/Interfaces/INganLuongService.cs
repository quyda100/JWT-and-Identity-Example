namespace auth.Interfaces
{
    public interface INganLuongService
    {
        public string BuildCheckOutURL(string transactionInfo, string orderId, int price);
        public bool VerifyPaymentUrl(string transaction_info, string order_code, string price, string payment_id, string payment_type, string error_text, string secure_code);
    }
}
