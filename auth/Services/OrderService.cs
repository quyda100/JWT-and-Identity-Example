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
        private readonly IMapper _mapper;


        public async Task<List<int>> getDataOrder()
        {
            for (int i = 1; i < 12; i++)
            {
                _context.Orders.Sum(o => o.Total);
            }
            var dataDash =  _context.Orders.Where(o => o.PaymentTime.Year == DateTime.Now.Year).Sum(o => o.Total);
            
        }
    }
}
