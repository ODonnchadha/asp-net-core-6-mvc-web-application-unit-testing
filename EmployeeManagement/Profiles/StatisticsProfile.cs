using AutoMapper;
using Microsoft.AspNetCore.Http.Features;

namespace EmployeeManagement.Profiles
{
    public class StatisticsProfile : Profile
    {
        public StatisticsProfile()
        {
            CreateMap<IHttpConnectionFeature, ViewModels.StatisticsViewModel>();
        }
    }
}
