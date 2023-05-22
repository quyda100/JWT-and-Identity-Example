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
            var count = _context.Orders.Where(o => o.CreatedAt.Date == DateTime.Now.Date).Count();
            return count;
        }

        public long DailyOrderSales()
        {
            var sales = _context.Orders.Where(o => o.CreatedAt.Date == DateTime.Now.Date && o.Status != -1).Sum(o => o.Total);
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
            var products = _context.Products.OrderByDescending(p => p.Sales).Take(4).Select(p => _mapper.Map<ProductDTO>(p)).ToList();
            return products;
        }

        public List<object> GetYearlySales()
        {
            List<object> result = new List<object>();
            for (int i = 1; i <= 12; i++)
            {
                int count = 0;
                var orders = _context.Orders.Where(o => o.CreatedAt.Year == DateTime.Now.Year && o.CreatedAt.Month == i).Include(o => o.OrderProducts).ToList();
                foreach (var item in orders)
                {
                    count += item.OrderProducts.Sum(o => o.Quantity);
                }
                result.Add(new { month = $"Tháng {i}", value = count });
            }
            return result;
        }

        public int UsersCount()
        {
            return _context.Users.Count();
        }
        public long OrderSalesTotalMonth(int month)
        {
            var orders = _context.Orders.Where(o => o.CreatedAt.Month == month && o.CreatedAt.Year == DateTime.Now.Year);
            long total = orders.Where(o => o.PaymentTime != null).Sum(o => o.Total);
            return total;
        }
        public long ImportTotalMonth(int month)
        {
            var imports = _context.Imports.Where(i => i.CreatedAt.Month == month && i.CreatedAt.Year == DateTime.Now.Year);
            var total = imports.Sum(i => i.Total);
            return total;
        }
        public int CountOrdersMonth(int month)
        {
            var orders = _context.Orders.Where(o => o.CreatedAt.Month == month && o.CreatedAt.Year == DateTime.Now.Year);
            var count = orders.Where(o => o.PaymentTime != null).Count();
            return count;
        }
        public List<object> TotalProductsCategoryOfWeek()
        {
            var startOfWeek = DateTime.Now.AddDays(DayOfWeek.Sunday - DateTime.Now.DayOfWeek);
            List<object> result = new List<object>();
            for (int i = 0; i <= 6; i++)
            {
                var orders = _context.Orders.Where(o => o.CreatedAt.Date == startOfWeek.AddDays(i).Date && o.Status != -1 && o.Status != -2).Include(o => o.OrderProducts).ThenInclude(p => p.Product).ToList();
                int sumMale = 0;
                int sumFemale = 0;
                foreach (var item in orders)
                {
                    sumMale = item.OrderProducts.Where(p => p.Product.CategoryId == 1).Sum(p => p.Quantity);
                    sumFemale = item.OrderProducts.Where(p => p.Product.CategoryId == 2).Sum(p => p.Quantity);
                }
                result.Add(new { date = GetDayOfWeek(i), key = "Nam", value = sumMale });
                result.Add(new { date = GetDayOfWeek(i), key = "Nữ", value = sumFemale });
            }
            return result;
        }
        public List<object> BrandCountSales(int month)
        {
            var result = new List<object>();
            var orders = _context.Orders.Where(o => o.PaymentTime != null && o.CreatedAt.Month == month && o.CreatedAt.Year == DateTime.Now.Year).Include(o => o.OrderProducts).ThenInclude(p => p.Product).ToList();
            var brands = _context.Brands.ToList();
            foreach (var brand in brands)
            {
                int count = 0;
                foreach (var order in orders)
                {
                    count += order.OrderProducts.Where(p => p.Product.BrandId == brand.Id).Count();
                }
                result.Add(new { type = brand.Name, sales = count });
            }
            return result;
        }

        public List<object> BrandCountStock()
        {
            var result = new List<object>();
            var brands = _context.Brands.Include(b => b.Products).ToList();
            foreach (var brand in brands)
            {
                result.Add(new { type = brand.Name, value = brand.Products.Sum(p => p.Stock) });
            }
            return result;
        }
        private string GetDayOfWeek(int day)
        {
            switch (day)
            {
                case 0: return "Chủ Nhật";
                case 1: return "Thứ Hai";
                case 2: return "Thứ Ba";
                case 3: return "Thứ Tư";
                case 4: return "Thứ Năm";
                case 5: return "Thứ Sáu";
                case 6: return "Thứ Bảy";
                default: return "";
            }
        }
    }
}
