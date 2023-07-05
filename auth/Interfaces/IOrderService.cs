using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IOrderService
    {
        public Task<int> CreateOrder(OrderRequest order);
        public List<OrderDTO> GetOrders();
        public List<OrderProductDTO> GetOrderProducts(int orderId);
        public Task UpdateOrder(int id, OrderDTO order);
        public Task UpdateOrderStatus(List<int> ids, int status);
        public List<OrderDTO> GetOrdersByUserId();
        public void DeleteOrder(int id);
        public Task<Order> FindOrder(int id);
        public Task UpdateOrderCheckout(int id, long price);
        public Task UpdateOrderCheckout(int id);
        public void UpdateOrderGHN(GHNOrderRequest request);
    }
}
