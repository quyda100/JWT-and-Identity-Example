using auth.Model;

namespace auth.Interfaces
{
    public interface IOrderService
    {
        public void addOrder(Order order);
        //public Task<List<int>> getDataOrder();
    }
}
