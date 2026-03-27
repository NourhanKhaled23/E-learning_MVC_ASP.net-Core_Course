using AutoMapper;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseViewModel>().ReverseMap();
        }
    }
}
