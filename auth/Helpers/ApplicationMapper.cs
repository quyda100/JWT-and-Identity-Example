using auth.Model;
using auth.Model.ViewModel;
using AutoMapper;

namespace auth.Helpers
{
    public class ApplicationMapper:Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Product, ProductDTO>();
        }
    }
}
