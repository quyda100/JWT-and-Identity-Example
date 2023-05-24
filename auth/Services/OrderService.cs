using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Security.Principal;

namespace auth.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _log;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogService log, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _log = log;
            _mapper = mapper;
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
                var product = _context.Products.FirstOrDefault(p => p.Id == productRequest.ProductId);
                if (product == null)
                {
                    throw new Exception("Không tìm thấy sản phẩm: " + productRequest.ProductId);
                }
                if (product.Stock < productRequest.Quanlity)
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

        public List<OrderDTO> GetOrders()
        {
            var orders = _context.Orders.Include(o => o.OrderProducts).ThenInclude(p => p.Product).Include(o => o.User).Select(o => _mapper.Map<OrderDTO>(o)).ToList();
            return orders;
        }

        public List<OrderProductDTO> GetOrderProducts(int orderId)
        {
            var orderProducts = _context.OrderProducts.Where(o => o.OrderId == orderId).Include(o => o.Product).Select(o => _mapper.Map<OrderProductDTO>(o)).ToList();
            return orderProducts;
        }

        public void UpdateOrder(int id, OrderDTO model)
        {
            if (id != model.Id)
                throw new Exception("Having a trouble");
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                throw new Exception("Không tìm thấy hóa đơn");
            }
            order.Status = model.Status;
            if (order.Status == 1)
            {
                UpdateProductSales(order.OrderProducts);
            }
            order.UpdatedAt = DateTime.Now;
            _log.SaveLog("Cập nhật đơn hàng: " + id);
            _context.Orders.Update(order);
        }

        public List<OrderDTO> GetOrdersByUserId()
        {
            var orders = _context.Orders.Where(o => o.UserId == GetUserId()).Include(o => o.OrderProducts).ThenInclude(p => p.Product).Include(o => o.User).Select(o => _mapper.Map<OrderDTO>(o)).ToList();
            return orders;
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id && o.UserId == GetUserId()) ?? throw new Exception("Không tìm thấy hóa đơn");
            if (order.Status != 0)
            {
                throw new Exception("Không thể thực hiện yêu cầu");
            }
            order.Status = -1;
            order.UpdatedAt = DateTime.Now;
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
        private void UpdateProductSales(List<OrderProduct> orderProducts)
        {
            foreach (var item in orderProducts)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                product.Stock -= item.Quantity;
                product.Sales += item.Quantity;
                _context.Products.Update(product);
            }
            _context.SaveChanges();
        }
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public void UpdateOrderStatus(List<int> ids, int status)
        {
            foreach (var id in ids)
            {
                var order = _context.Orders.FirstOrDefault(o => o.Id == id);
                if (order == null)
                {
                    throw new Exception("Không tìm thấy hóa đơn: " + id);
                }
                order.Status = status;
                _context.Orders.Update(order);
                _context.SaveChanges();
                _log.SaveLog($"Cập nhật hóa đơn: {id} thành {status}");
            }
        }
    }
}
