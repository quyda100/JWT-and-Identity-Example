using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Security.Principal;

namespace auth.Services
{
    public class OrderService: IOrderService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _log;

        public OrderService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogService log)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
        }

        public void CreateOrder(OrderRequest model)
        {
            var order = new Order
            {
                CustomerName = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Status = 0,
                UserId = GetUserId(),
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
                if(product == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm: " + productRequest.ProductId);
                }
                if(product.Stock < productRequest.Quanlity)
                {
                    throw new Exception("Sản phẩm: " + product.Code + " tồn kho không đủ");
                }
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
            if (Order == null)
            {
                throw new Exception("Không tìm thấy hóa đơn");
            }
            order.UpdatedAt = DateTime.Now;
            _log.SaveLog("Cập nhật đơn hàng: " + id);
            _context.Orders.Update(order);
        }

        public List<Order> GetOrdersByUserId()
        {
            var orders = _context.Orders.Where(o => o.UserId == GetUserId()).Include(o=>o.OrderProducts).Include(o=>o.User).ToList();
            return orders;
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id&&o.UserId == GetUserId()) ?? throw new Exception("Không tìm thấy hóa đơn");
            if (order.Status != 0)
            {
                throw new Exception("Không thể thực hiện yêu cầu");
            }
            order.Status = -1;
            order.UpdatedAt = DateTime.Now;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
