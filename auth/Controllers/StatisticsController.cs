using auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _service;

        public StatisticsController(IStatisticService service)
        {
            _service = service;
        }
        [HttpGet("DailyOrderSales")]
        public IActionResult GetDailyOrderSales()
        {
            return Ok(_service.DailyOrderSales());
        }
        [HttpGet("UsersCount")]
        public IActionResult GetUsersCount()
        {
            return Ok(_service.UsersCount());
        }
        [HttpGet("DailyOrderCount")]
        public IActionResult GetDailyOrderCount()
        {
            return Ok(_service.DailyOrderCount());
        }
        [HttpGet("DailyProductSales")]
        public IActionResult GetDailyProductSales()
        {
            return Ok(_service.DailyProductSales());
        }
        [HttpGet("GetYearlySales")]
        public IActionResult GetYearlySales()
        {
            return Ok(_service.GetYearlySales());
        }
        [HttpGet("GetBestProductsSale")]
        public IActionResult GetBestProductsSale()
        {
            return Ok(_service.GetBestProductsSale());
        }
        [HttpGet("TotalProductsCategoryOfWeek")]
        public IActionResult TotalProductsCategoryOfWeek()
        {
            return Ok(_service.TotalProductsCategoryOfWeek());
        }
        [HttpGet("OrderSalesTotalMonth/{month}")]
        public IActionResult OrderSalesTotalMonth(int month)
        {
            return Ok(_service.OrderSalesTotalMonth(month));
        }
        [HttpGet("CountOrdersMonth/{month}")]
        public IActionResult CountOrdersMonth(int month)
        {
            return Ok(_service.CountOrdersMonth(month));
        }
        [HttpGet("ImportTotalMonth/{month}")]
        public IActionResult ImportTotalMonth(int month)
        {
            return Ok(_service.ImportTotalMonth(month));
        }
        [HttpGet("BrandCountSales/{month}")]
        public IActionResult BrandCountSales(int month)
        {
            return Ok(_service.BrandCountSales(month));
        }
        [HttpGet("BrandCountStock")]
        public IActionResult BrandCountStock()
        {
            return Ok(_service.BrandCountStock());
        }
    }
}
