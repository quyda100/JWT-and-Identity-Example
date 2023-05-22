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
        public List<object> TotalProductsCategoryOfWeek();

        public long OrderSalesTotalMonth(int month);
        public long ImportTotalMonth(int month);
        public int CountOrdersMonth(int month);
        public List<object> BrandCountSales(int month);
        public List<object> BrandCountStock();

    }
}
