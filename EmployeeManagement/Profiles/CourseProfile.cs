using AutoMapper;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, ViewModels.CourseViewModel>();
        }
    }
}
