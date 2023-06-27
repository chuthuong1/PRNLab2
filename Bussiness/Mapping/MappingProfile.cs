using WebApplication1.Bussiness.DTO;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Bussiness.Mapping
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(des => des.Category,
                act => act.MapFrom(src => src.Category));
        }
    }
}
