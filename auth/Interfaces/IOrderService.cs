using auth.Model;

namespace auth.Interfaces
{
    public interface IOrderService
    {
        public void AddOrder(Order order);
        public void CreateOrder(OrderRequest order);
        //public Task<List<int>> getDataOrder();
    }
}
