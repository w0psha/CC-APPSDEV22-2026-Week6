using AutoMapper;
using StudentRosterDbApi.DTOs;
using StudentRosterDbApi.Models;

namespace StudentRosterDbApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : "Unknown"));

            CreateMap<StudentCreateDto, Student>();
        }
    }
}