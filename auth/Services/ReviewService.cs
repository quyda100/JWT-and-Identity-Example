using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace auth.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogService _log;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewService(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor, ILogService log)
        {
            _context = context;
            _log = log;
            _httpContextAccessor = httpContextAccessor;
        }
        public void AddReview(Review model)
        {
            model.UserId = GetUserId();
            _context.Reviews.Add(model);
            _context.SaveChanges();
        }

        public void DeleteReview(int id)
        {
            var review = FindReview(id);
            review.IsDeleted = true;
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Review>> GetReviews(int ProductId)
        {
            var reviews = await _context.Reviews.Where(x => x.ProductId == ProductId).ToListAsync();
            return reviews;
        }

        public void UpdateReview(int id, Review model)
        {
            if (model.Id != id)
                throw new Exception("Having trouble");
            _context.Reviews.Update(model);
            _context.SaveChanges();
        }

        private Review FindReview(int id)
        {
            var review = _context.Reviews.SingleOrDefault(x => x.Id == id);
            if (review == null)
            {
                throw new Exception("Review not found");
            }
            return review;
        }
        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
