using auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutsController : ControllerBase
    {
        private readonly INganLuongService _service;
        private readonly IOrderService _orderService;

        public CheckoutsController(INganLuongService service, IOrderService orderService) {
            _service = service;
            _orderService = orderService;
        }
        [HttpGet("GetURL")]
        public IActionResult BuildURL(string transactionInfo, int orderId, int price)
        {
            return Ok(_service.BuildCheckOutURL(transactionInfo, orderId.ToString(), price));
        }
        [HttpGet("Receiver")]
        public async Task<IActionResult> VerifyURL (string transaction_info, string order_code, string price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            bool result = _service.VerifyPaymentUrl(transaction_info, order_code, price, payment_id, payment_type, error_text, secure_code);
            try
            {
                if (result && error_text == string.Empty)
                {
                    await _orderService.UpdateOrderCheckout(int.Parse(order_code), long.Parse(price));
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string order_code, string payment_id, string payment_type, string secure_code, string transaction_info)
        {
            try
            {
                var result = _service.UpdateOrder(order_code,payment_id,payment_type,secure_code,transaction_info);
                if (result)
                {
                    await _orderService.UpdateOrderCheckout(int.Parse(order_code));
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
