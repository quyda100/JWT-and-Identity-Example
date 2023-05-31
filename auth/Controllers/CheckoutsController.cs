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
            return Ok(_service.BuildCheckOutURL(transactionInfo, orderId.ToString(), price.ToString()));
        }
        [HttpGet("Receiver")]
        public IActionResult GetURL (string transaction_info, string order_code, string price, string payment_id, string payment_type, string error_text, string secure_code)
        {
            bool result = _service.VerifyPaymentUrl(transaction_info, order_code, price, payment_id, payment_type, error_text, secure_code);
            return result ? Ok(result) : BadRequest(error_text);
        }
    }
}
