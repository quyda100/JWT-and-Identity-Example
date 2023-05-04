using auth.Interfaces;
using auth.Model;
using auth.Model.Request;
using auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogService _log;
        public OrdersController(IOrderService service, ILogService log)
        {
            _service = service;
            _log = log;
        }

        [HttpPost("CreateOrder")]
        [AllowAnonymous]
        public IActionResult CreateOrder(OrderRequest order)
        {
            _service.CreateOrder(order);
            return Ok(new { Message = "Tạo hóa đơn thành công" });
        }
        [HttpGet]
        public IActionResult getListOrders()
        {
            return Ok(_service.GetOrders());
        }

        [HttpPut("{id}")]
        public IActionResult updateOrder(int id, Order order)
        {
            _service.UpdateOrder(id, order);
            return Ok(new { message = "Cập nhật thành công" });
        }
    }
}
