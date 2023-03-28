using auth.Data;
using auth.Interfaces;
using auth.Model;
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

        // add hoa don
        public void AddOrder(Order order)
        {
            try
            {
                if (_context.Products.Any(x => x.Name == order.UserName))
                    throw new Exception(order.UserName + " is exist");
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }

        }

        
        
        //public task<list<int>> getdataorder()
        //{
        //    list<int> lst = new list<int>();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        lst.add(_context.orders.where(o => o.paymenttime.month == i && o.paymenttime.year == datetime.now.year).sum(o => o.total));
        //    }
        //    return task.fromresult(lst);
        //}
        public void CreateOrder(OrderRequest model)
        {
            var order = new Order
            {
                UserName = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Status = 0
            };
            _context.Orders.Add(order);
            order.Total = model.orderProducts.Sum(p => p.Quanlity * p.Price); // Tổng hóa đơn
            _context.SaveChanges();
            //Tạo orderProduct
            var orderProducts = GetOrderProducts(model.orderProducts, order.Id);
            _context.OrderProducts.AddRange(orderProducts);

            _context.SaveChanges();
        }
        public async Task<Order> AddOrderProducts(Order order)
        {
            await _context.Orders.AddAsync(order);
            _context.SaveChanges();
            return order;
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
    }
}
