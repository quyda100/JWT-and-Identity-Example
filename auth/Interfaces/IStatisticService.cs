using auth.Model.DTO;

namespace auth.Interfaces
{
    public interface IStatisticService
    {
        public long DailyOrderSales();
        public int UsersCount();
        public int DailyOrderCount();
        public long DailyProductSales();

        public List<ProductDTO> GetBestProductsSale();
        public List<Object> GetYearlySales();
    }
}
