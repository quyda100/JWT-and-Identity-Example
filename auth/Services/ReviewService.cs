using auth.Data;
using auth.Interfaces;
using auth.Model;
using auth.Model.DTO;
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
        public void AddReview(string content, int productId)
        {
            var review = new Review{
                Content = content,
                UserId = GetUserId(),
                ProductId = productId
            };
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public  List<ReviewDTO> GetReviews(int ProductId)
        {
            var reviews = _context.Reviews.Where(x => x.ProductId == ProductId)
                    .Include(x=>x.User).Select(x=> new ReviewDTO {
                        Id = x.Id,
                        Avatar = x.User.Avatar,
                        Content = x.Content,
                        CreatedDate = x.CreatedDate,
                        UserName = x.User.FullName
                    }).ToList();
            return reviews;
        }

        private string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
