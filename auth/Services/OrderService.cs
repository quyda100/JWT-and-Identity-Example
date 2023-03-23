using auth.Data;
using auth.Interfaces;
using auth.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public void addOrder(Order order)
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

        public Task<List<int>> getDataOrder()
        {
            List<int> lst = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                lst.Add(_context.Orders.Where(o => o.PaymentTime.Month == i && o.PaymentTime.Year == DateTime.Now.Year).Sum(o => o.Total));
            }
            return Task.FromResult(lst);
        }
    }
}
