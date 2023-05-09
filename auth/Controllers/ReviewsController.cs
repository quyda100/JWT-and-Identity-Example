using auth.Interfaces;
using auth.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;
        public ReviewsController(IReviewService service, ILogService log)
        {
            _service = service;
        }
        [HttpGet("getReviews")]
        public IActionResult getReview(int id)
        {
            var reviews = _service.getReviews(id);
            return Ok(new{ status = "success", data = reviews, message = "Lấy dữ liệu thành công" });
        }
        [HttpPost("addReview")]
        public IActionResult addReview(Review Review)
        {
            _service.addReview(Review);
            return Ok(new{ status = "success", message = "Thêm review thành công" });
        }
        [HttpPost("updateReview")]
        public IActionResult updateReview(int id, Review Review)
        {
            _service.updateReview(id, Review);
            return Ok(new{ status = "success", message = "Cập nhật review thành công" });
        }
        [HttpPost("deleteReview")]
        public IActionResult deleteReview(int id)
        {
            _service.deleteReview(id);
            return Ok(new{ status = "success", message = "Xóa review thành công" });
        }
    }
}
