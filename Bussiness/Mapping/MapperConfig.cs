using AutoMapper;

namespace WebApplication1.Bussiness.Mapping
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            return new Mapper(config);
        }
    }
}
