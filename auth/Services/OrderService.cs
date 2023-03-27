using auth.Data;
using auth.Interfaces;
using auth.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Order> AddOrderProducts(Order order)
        {
            await _context.Orders.AddAsync(order);
            _context.SaveChanges();
            return order;
        }
        
    }
}
