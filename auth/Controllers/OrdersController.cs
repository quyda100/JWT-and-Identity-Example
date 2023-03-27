using auth.Interfaces;
using auth.Model;
using auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("AddProduct")]
        [Authorize]
        public IActionResult Add(Order order)
        {
            _service.AddOrder(order);
            return Ok(new { message = "Thêm hoa don thành công" });
        }

        //[HttpGet]
        //[Route("getListOrders")]
        //public IActionResult getListOrders()
        //{
        //    return Ok(_service.getDataOrder());
        //}
    }
}
