using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using auth.Model.Request;
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

        [HttpPost("CreateOrder")]
        [Authorize(Roles = UserRoles.User)]
        public IActionResult CreateOrder(OrderRequest order)
        {
            try
            {
                if (order == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.CreateOrder(order);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public IActionResult GetListOrders()
        {
            return Ok(_service.GetOrders());
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, OrderDTO order)
        {
            try
            {
                if (order == null || !ModelState.IsValid)
                {
                    return BadRequest("Vui lòng nhập đúng thông tin");
                }
                _service.UpdateOrder(id, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(List<int> id, int status)
        {
            try
            {
                if (id.Count <= 0)
                {
                    return BadRequest("Vui lòng chọn hóa đơn");
                }
                _service.UpdateOrderStatus(id, status);
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
            var orders = _service.GetOrdersByUserId();
            return Ok(orders);
        }
        [HttpPost("DeleteOrder")]
        [Authorize(Roles = UserRoles.User)]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                _service.DeleteOrder(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

    }
}
