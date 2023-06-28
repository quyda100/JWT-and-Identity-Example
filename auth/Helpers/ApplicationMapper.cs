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
            CreateMap<Order, OrderDTO>().ForMember(o=>o.FullName, od => od.MapFrom(o=>o.User.FullName));
            CreateMap<OrderProduct, OrderProductDTO>();
            CreateMap<Import, ImportDTO>().ForMember(i => i.FullName, id => id.MapFrom(i=>i.User.FullName));
            CreateMap<ImportDetail, ImportProductDTO>();
            CreateMap<New, NewDTO>().ForMember(n => n.UserName, nd => nd.MapFrom(n=>n.User.FullName));
            CreateMap<NewDTO, New>();
            CreateMap<Log, LogDTO>().ForMember(i => i.FullName, id => id.MapFrom(i=>i.User.FullName));
        }
    }
}
