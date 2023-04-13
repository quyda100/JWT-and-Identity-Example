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

        public ReviewsController(IReviewService service)
        {
            _service = service;
        }
        [HttpGet("getReviews")]
        public IActionResult getReview(int id)
        {
            return Ok(_service.getReviews(id));
        }
        [HttpPost("addReview")]
        public IActionResult addReview(Review Review)
        {
            _service.addReview(Review);
            return Ok(new { message = "Thành công" });
        }
        [HttpPost("updateReview")]
        public IActionResult updateReview(int id, Review Review)
        {
            _service.updateReview(id, Review);
            return Ok(new { message = "Thành công" });
        }
        public IActionResult deleteReview(int id)
        {
            _service.deleteReview(id);
            return Ok(new { message = "Thành công" });
        }
    }
}
