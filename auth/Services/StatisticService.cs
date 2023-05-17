using auth.Data;
using auth.Interfaces;
using auth.Model.DTO;
using AutoMapper;

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
            var count = _context.Orders.Where(o=>o.CreatedAt.Day == DateTime.Now.Day).Count();
            return count;
        }

        public long DailyOrderSales()
        {
            var sales = _context.Orders.Where(o => o.CreatedAt.Day == DateTime.Now.Day && o.Status == 3).Sum(o=>o.Total);
            return sales;
        }

        public long DailyProductSales()
        {
            throw new NotImplementedException();
        }

        public List<ProductDTO> GetBestProductsSale()
        {
            throw new NotImplementedException();
        }

        public List<object> GetYearlySales()
        {
            throw new NotImplementedException();
        }

        public int UsersCount()
        {
            throw new NotImplementedException();
        }
    }
}
