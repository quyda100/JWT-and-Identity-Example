using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IOrderService
    {
        public void CreateOrder(OrderRequest order);
        public List<OrderDTO> GetOrders();
        public List<OrderProductDTO> GetOrderProducts(int orderId);
        public void UpdateOrder(int id,OrderDTO order);
        public void UpdateOrderStatus(int id, int status);
        public void UpdateOrderStatus(List<int> ids, int status);
        public List<OrderDTO> GetOrdersByUserId();
        public void DeleteOrder(int id);

    }
}
