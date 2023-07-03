using auth.Model;
using auth.Model.DTO;

namespace auth.Interfaces
{
    public interface IReviewService
    {
        public List<ReviewDTO> GetReviews(int ProductId);
        public void AddReview(string content, int productId);
    }
}
