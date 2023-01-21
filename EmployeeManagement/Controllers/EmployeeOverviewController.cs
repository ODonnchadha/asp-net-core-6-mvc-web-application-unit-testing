using AutoMapper;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    public class EmployeeOverviewController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeOverviewController(IEmployeeService employeeService, 
            IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [Authorize()]
        public IActionResult ProtectedIndex()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminIndex", "EmployeeManagement");
            }

            return RedirectToAction("Index", "EmployeeManagement");
        }

        public async Task<IActionResult> Index()
        {
            var internalEmployees = await _employeeService
                .FetchInternalEmployeesAsync();

            // Manual Mapping:
            //var internalEmployeeForOverviewViewModels =
            //    internalEmployees.Select(e => 
            //        new InternalEmployeeForOverviewViewModel()
            //        {
            //            Id = e.Id,
            //            FirstName = e.FirstName,
            //            LastName = e.LastName,
            //            Salary = e.Salary,
            //            SuggestedBonus = e.SuggestedBonus,
            //            YearsInService = e.YearsInService
            //        });

            // AutoMapper:
            var internalEmployeeForOverviewViewModels =
                _mapper.Map<IEnumerable<InternalEmployeeForOverviewViewModel>>(internalEmployees);

            return View(
                new EmployeeOverviewViewModel(internalEmployeeForOverviewViewModels));
        }
         

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}