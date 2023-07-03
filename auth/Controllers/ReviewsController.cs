using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.User)]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _service;
        public ReviewsController(IReviewService service, ILogService log)
        {
            _service = service;
        }
        [AllowAnonymous]
        [HttpGet("getReviews")]
        public IActionResult GetReview(int productId)
        {
            var reviews = _service.GetReviews(productId);
            return Ok(reviews);
        }
        [HttpPost("addReview")]
        public IActionResult AddReview(string content, int productId)
        {
            try
            {
                _service.AddReview(content, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
