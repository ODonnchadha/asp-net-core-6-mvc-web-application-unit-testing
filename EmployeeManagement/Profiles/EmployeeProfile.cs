using AutoMapper;
using EmployeeManagement.Entities;

namespace EmployeeManagement.Profiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<InternalEmployee, ViewModels.InternalEmployeeForOverviewViewModel>();
            CreateMap<InternalEmployee, ViewModels.InternalEmployeeDetailViewModel>(); 
        }
    }
}
