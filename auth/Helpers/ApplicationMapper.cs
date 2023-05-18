using auth.Model;
using auth.Model.DTO;
using AutoMapper;

namespace auth.Helpers
{
    public class ApplicationMapper:Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderProduct, OrderProductDTO>();
        }
    }
}
