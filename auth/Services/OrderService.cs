using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Principal;

namespace auth.Services
{
    public class OrderService: IOrderService
    {
        private readonly ApplicationDBContext _context;

        public OrderService(ApplicationDBContext context)
        {
            _context = context;
        }

        public void CreateOrder(OrderRequest model)
        {
            var order = new Order
            {
                CustomerName = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Status = 0
            };
            _context.Orders.Add(order);
            
            _context.SaveChanges();
            //Tạo orderProduct
            var orderProducts = GetOrderProducts(model.orderProducts, order.Id);
            order.Total = orderProducts.Sum(p => p.Quantity * p.Price); // Tổng hóa đơn
            _context.OrderProducts.AddRange(orderProducts);

            _context.SaveChanges();
        }
        public List<OrderProduct> GetOrderProducts(List<OrderProductRequest> productRequests, int OrderId)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            foreach (var productRequest in productRequests)
            {
                var product = _context.Products.First(p => p.Id == productRequest.ProductId);
                if(product == null) continue;
                var orderProduct = new OrderProduct
                {
                    ProductId = product.Id,
                    OrderId = OrderId,
                    Price = product.Price,
                    Quantity = productRequest.Quanlity
                };
                orderProducts.Add(orderProduct);
            }
            return orderProducts;
        }

        public List<Order> GetOrders()
        {
           var orders = _context.Orders.ToList();
            return orders;
        }

        public List<OrderProduct> GetOrderProducts(int orderId)
        {
            var orderProducts = _context.OrderProducts.Where(o=>o.OrderId==orderId).ToList();
            return orderProducts;
        }

        public void UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
                throw new Exception("Having a trouble");
            var Order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.UpdatedAt = DateTime.Now;
            _context.Orders.Update(order);
        }
    }
}
