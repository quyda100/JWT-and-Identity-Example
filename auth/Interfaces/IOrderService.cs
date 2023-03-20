using auth.Model;

namespace auth.Interfaces
{
    public interface IOrderService
    {
        public Task<List<int>> getDataOrder();
    }
}
