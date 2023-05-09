using auth.Model;

namespace auth.Interfaces
{
    public interface IReviewService
    {
        public Task<IEnumerable<Review>> getReviews(int ProductId);
        public void addReview(Review model);
        public void updateReview(int id, Review model);
        public void deleteReview(int id);
    }
}
