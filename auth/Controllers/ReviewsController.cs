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
        public IActionResult GetReview(int id)
        {
            var reviews = _service.GetReviews(id);
            return Ok(reviews);
        }
        [HttpPost("addReview")]
        public IActionResult AddReview(Review review)
        {
            try
            {
                _service.AddReview(review);
                return Ok(review);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("updateReview")]
        public IActionResult UpdateReview(int id, Review Review)
        {
            try
            {
                _service.UpdateReview(id, Review);
            }
            catch (Exception)
            {

                throw;
            }
            return NoContent();
        }
        [HttpPost("deleteReview")]
        public IActionResult deleteReview(int id)
        {
            try
            {
                _service.DeleteReview(id);
            }
            catch (Exception)
            {

                throw;
            }
            return NoContent();
        }
    }
}
