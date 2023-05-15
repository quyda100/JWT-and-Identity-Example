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
        [Authorize(Roles = UserRoles.User)]
        public IActionResult CreateOrder(OrderRequest order)
        {
            try
            {
                if(order == null||!ModelState.IsValid) {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                var userId = GetCurrentUserId();
                _service.CreateOrder(order, userId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetListOrders()
        {
            return Ok(_service.GetOrders());
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, Order order)
        {
            try
            {
                if(order==null ||!ModelState.IsValid) {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                var userId = GetCurrentUserId();
                _service.UpdateOrder(id, order);
                _log.saveLog(new Log { UserId = userId, Action = "Cập nhật trạng thái đơn hàng: " + order.Id});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpGet("GetOrdersByUserId")]
        [Authorize(Roles = UserRoles.User)]
        public IActionResult GetOrdersByUserId()
        {
            var userId = GetCurrentUserId() ;
            var orders = _service.GetOrdersByUserId(userId);
            return Ok(orders);
        }
        [HttpPost("DeleteOrder")]
        [Authorize(Roles = UserRoles.User)]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                _service.DeleteOrder(id, userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        private string GetCurrentUserId()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            if (userId == null)
            {
                throw new Exception("Vui lòng đăng nhập lại");
            }
            return userId;
        }
    }
}
