using auth.Model;
using auth.Model.DTO;
using AutoMapper;

namespace auth.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderProduct, OrderProductDTO>();
            CreateMap<Import, ImportDTO>().ForMember(i => i.FullName, id => id.MapFrom(i=>i.User.FullName));
            CreateMap<ImportDetail, ImportProductDTO>();
            CreateMap<New, NewDTO>();
            CreateMap<NewDTO, New>();
            CreateMap<Log, LogDTO>();
        }
    }
}
