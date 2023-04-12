using auth.Data;
using auth.Interfaces;
using auth.Model;
using Microsoft.EntityFrameworkCore;

namespace auth.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDBContext _context;

        public ReviewService(ApplicationDBContext context) { 
            _context = context;
        }
        public void addReview(Review model)
        {
            try
            {
                _context.Reviews.Add(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        public void deleteReview(int id)
        {
            var review = findReview(id);
            review.IsDeleted = true;
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Review>> getReviews(int ProductId)
        {
            var reviews = await _context.Reviews.Where(x=>x.ProductId == ProductId).ToListAsync();
            return reviews;
        }

        public void updateReview(int id, Review model)
        {
            if (p.Id != id)
                throw new Exception("Having trouble");
            _context.Reviews.Update(model);
            _context.SaveChanges();
        }

        private Review findReview(int id) { 
            var review = _context.Reviews.SingleOrDefault(x => x.Id == id);
            if(review == null)
            {
                throw new Exception("Review not found");
            }
            return review;
        }
    }
}
