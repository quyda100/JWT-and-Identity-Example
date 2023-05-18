using auth.Data;
using auth.Interfaces;
using auth.Model.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public StatisticService(ApplicationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int DailyOrderCount()
        {
            var count = _context.Orders.Where(o=>o.CreatedAt.Date == DateTime.Now.Date).Count();
            return count;
        }

        public long DailyOrderSales()
        {
            var sales = _context.Orders.Where(o => o.CreatedAt.Date == DateTime.Now.Date && o.Status != -1 && o.Status != -2).Sum(o=>o.Total);
            return sales;
        }

        public long DailyProductSales()
        {
            long sum = 0;
            var orders = _context.Orders.Where(o => o.CreatedAt.Date == DateTime.Now.Date).Include(o => o.OrderProducts).ToList();
            foreach (var item in orders)
            {
                sum += item.OrderProducts.Sum(o => o.Quantity);
            }
            return sum;
        }

        public List<ProductDTO> GetBestProductsSale()
        {
            var products = _context.Products.OrderByDescending(p=>p.Sales).Select(p=>_mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }

        public List<object> GetYearlySales()
        {
            List<object> result = new List<object>();
            for (int i = 1; i <= 12; i++)
            {
                int count = 0;
                var orders = _context.Orders.Where(o => o.CreatedAt.Year == DateTime.Now.Year && o.CreatedAt.Month == i).Include(o=>o.OrderProducts).ToList();
                foreach (var item in orders)
                {
                    count += item.OrderProducts.Sum(o=>o.Quantity);
                }
                result.Add(new {Month = i, Count = count});
            }
            return result;
        }

        public int UsersCount()
        {
            return _context.Users.Count();
        }
    }
}
