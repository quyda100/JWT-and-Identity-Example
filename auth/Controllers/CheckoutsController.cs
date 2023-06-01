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

        public CheckoutsController(INganLuongService service) {
            _service = service;
        }
        [HttpGet("GetURL")]
        public IActionResult BuildURL(string transactionInfo, int orderId, int price)
        {
            return Ok(_service.BuildCheckOutURL(transactionInfo, orderId.ToString(), price));
        }
        [HttpGet("Receiver")]
        public IActionResult GetURL (string transaction_info, string order_code, string price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            bool result = _service.VerifyPaymentUrl(transaction_info, order_code, price, payment_id, payment_type, error_text, secure_code);
            Console.WriteLine(result + error_text);
            return result ? Ok(result) : BadRequest(error_text);
        }
        [HttpGet("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(string order_code, string payment_id, string payment_type, string secure_code, string transaction_info)
        {
            try
            {
                await _service.UpdateOrder(order_code,payment_id,payment_type,secure_code,transaction_info);
            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }
    }
}
