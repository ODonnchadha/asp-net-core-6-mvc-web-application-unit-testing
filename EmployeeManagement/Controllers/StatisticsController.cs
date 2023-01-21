using AutoMapper;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IMapper mapper;
        public StatisticsController(IMapper mapper) => this.mapper = mapper;

        public IActionResult Index()
        {
            var httpConnectionFeature = 
                HttpContext.Features.Get<IHttpConnectionFeature>();

            return View(
                mapper.Map<StatisticsViewModel>(
                    httpConnectionFeature)); 
        }
    }
}
