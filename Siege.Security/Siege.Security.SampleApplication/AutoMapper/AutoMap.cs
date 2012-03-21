using AutoMapper;

namespace Siege.Security.SampleApplication.AutoMapper
{
    public class AutoMap
    {
        public static void Build()
        {
            Mapper.CreateMap<Application, Application>().ForMember(source => source.ID, opt => opt.Ignore());
            Mapper.CreateMap<Group, Group>()
                .ForMember(source => source.Application, opt => opt.Ignore())
                .ForMember(source => source.ID, opt => opt.Ignore());
            Mapper.CreateMap<User, User>()
                .ForMember(source => source.Application, opt => opt.Ignore())
                .ForMember(source => source.ID, opt => opt.Ignore());
            Mapper.CreateMap<Role, Role>()
                .ForMember(source => source.Application, opt => opt.Ignore())
                .ForMember(source => source.ID, opt => opt.Ignore());
        }
    }
}