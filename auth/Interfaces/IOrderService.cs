﻿using auth.Model;
using auth.Model.Request;

namespace auth.Interfaces
{
    public interface IOrderService
    {
        public void CreateOrder(OrderRequest order, string userId);
        public List<Order> GetOrders();
        public List<OrderProduct> GetOrderProducts(int orderId);
        public void UpdateOrder(int id,Order order);
        public List<Order> GetOrdersByUserId(string userId);
        public void DeleteOrder(int id, string userId);

    }
}
