using auth.Model;

namespace auth.Interfaces
{
    public interface IReviewService
    {
        public Task<IEnumerable<Review>> GetReviews(int ProductId);
        public void AddReview(Review model);
        public void UpdateReview(int id, Review model);
        public void DeleteReview(int id);
    }
}
