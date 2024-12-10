using AutoMapper;

namespace LoggingSystem.API.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Log, CreateLogDto>().ReverseMap();
            CreateMap<Log, LogDto>().ReverseMap();  
        }
    }
}
