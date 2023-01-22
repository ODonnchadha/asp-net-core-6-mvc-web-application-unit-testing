using AutoMapper;
using EmployeeManagement.Interfaces.Services;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class InternalEmployeeController : Controller
    {
        private readonly IEmployeeService employee;
        private readonly IMapper mapper;
        private readonly IPromotionService promotion;

        public InternalEmployeeController(IEmployeeService employee,
            IMapper mapper, IPromotionService promotion)
        {
            this.employee = employee;
            this.mapper = mapper;
            this.promotion= promotion;
        }

        [HttpGet]
        public IActionResult AddInternalEmployee()
        {
            return View(new CreateInternalEmployeeViewModel()); 
        }

        [HttpPost]
        public async Task<IActionResult> AddInternalEmployee(CreateInternalEmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                // create an internal employee entity with default values filled out
                // and the values the user inputted
                var internalEmplooyee =
                    await employee.CreateInternalEmployeeAsync(model.FirstName, model.LastName);

                // persist it
                await employee.AddInternalEmployeeAsync(internalEmplooyee);
            }

            return RedirectToAction("Index", "EmployeeOverview");
        }

        [HttpGet]
        public async Task<IActionResult> InternalEmployeeDetails([FromRoute(Name = "id")] Guid? employeeId)
        {
            if (!employeeId.HasValue)
            {
                if (Guid.TryParse(HttpContext?.Session?.GetString("EmployeeId"), out Guid sessionId))
                {
                    employeeId = sessionId;
                }
                else if (Guid.TryParse(TempData["EmployeeId"]?.ToString(), out Guid tmpId))
                {
                    employeeId = tmpId;
                }
                else
                {
                    return RedirectToAction("Index", "EmployeeOverview");
                }
            }

            var internalEmployee = await employee.FetchInternalEmployeeAsync(employeeId.Value); 
            if (internalEmployee == null)
            {
                return RedirectToAction("Index", "EmployeeOverview"); 
            }
             
            return View(mapper.Map<InternalEmployeeDetailViewModel>(internalEmployee));  
        }

        [HttpPost]
        public async Task<IActionResult> ExecutePromotionRequest(
            [FromForm(Name = "id")] Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "EmployeeOverview");
            }

            var e = await employee.FetchInternalEmployeeAsync(id.Value);

            if (e == null)
            {
                return RedirectToAction("Index", "EmployeeOverview");
            }

            if (await promotion.PromoteInternalEmployeeAsync(e))
            {
                ViewBag.PromotionRequestMessage = "Employee was promoted.";

                e = await employee.FetchInternalEmployeeAsync(e.Id);
            }
            else
            {
                ViewBag.PromotionRequestMessage = "Employee is not eligible for promotion.";
            }
 
            return View("InternalEmployeeDetails", 
                mapper.Map<InternalEmployeeDetailViewModel>(e));
        }
    }
}
