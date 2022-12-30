using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.Entities;
using EmployeeManagement.Factories;
using EmployeeManagement.Interfaces.Repositories;
using EmployeeManagement.Interfaces.Services;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private Guid[] _obligatoryCourseIds = {
            Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
            Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")
        };

        private readonly EmployeeFactory factory;
        private readonly IEmployeeManagementRepository repository;

        public event EventHandler<EmployeeIsAbsentEventArgs>? EmployeeIsAbsent;

        public EmployeeService(
            EmployeeFactory factory, IEmployeeManagementRepository repository)
        {
            this.factory = factory;
            this.repository = repository;
        }

        public async Task AttendCourseAsync(InternalEmployee employee, Course attendedCourse)
        {
            var alreadyAttendedCourse = 
                employee.AttendedCourses.Any(c => c.Id == attendedCourse.Id);

            if (alreadyAttendedCourse)
            {
                return;
            }

            employee.AttendedCourses.Add(attendedCourse);
            await repository.SaveChangesAsync();

            employee.SuggestedBonus = employee.YearsInService
                * employee.AttendedCourses.Count * 100;
        }

        public async Task GiveMinimumRaiseAsync(InternalEmployee employee)
        {
            employee.Salary += 100;
            employee.MinimumRaiseGiven = true;

            await repository.SaveChangesAsync();
        }

        public async Task GiveRaiseAsync(InternalEmployee employee, int raise)
        {
            if (raise < 100)
            {
                throw new EmployeeInvalidRaiseException(
                    "Invalid raise: raise must be higher than or equal to 100.", raise);
            }

            if (employee.MinimumRaiseGiven && raise == 100)
            {
                throw new EmployeeInvalidRaiseException(
                    "Invalid raise: minimum raise cannot be given twice.", raise);
            }

            if (raise == 100)
            {
                await GiveMinimumRaiseAsync(employee);
            }
            else
            {
                employee.Salary += raise;
                employee.MinimumRaiseGiven = false;

                await repository.SaveChangesAsync();
            }
        }

        public async Task<InternalEmployee?> FetchInternalEmployeeAsync(Guid employeeId)
        {
            var employee = await repository.GetInternalEmployeeAsync(employeeId);

            if (employee != null)
            {
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }

            return employee;
        }

        public async Task<IEnumerable<InternalEmployee>> FetchInternalEmployeesAsync()
        {
            var employees = await repository.GetInternalEmployeesAsync();

            foreach (var employee in employees)
            {
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }

            return employees;
        }

        public InternalEmployee? FetchInternalEmployee(Guid employeeId)
        {
            var employee = repository.GetInternalEmployee(employeeId);

            if (employee != null)
            {
                employee.SuggestedBonus = CalculateSuggestedBonus(employee);
            }

            return employee;
        }

        public InternalEmployee CreateInternalEmployee(string firstName, string lastName)
        {
            var employee = (InternalEmployee)factory.CreateEmployee(firstName, lastName);
            var obligatoryCourses = repository.GetCourses(_obligatoryCourseIds);

            obligatoryCourses.ForEach(c => employee.AttendedCourses.Add(c));
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);

            return employee;
        }

        public async Task<InternalEmployee> CreateInternalEmployeeAsync(string firstName, string lastName)
        {
            var employee = (InternalEmployee)factory.CreateEmployee(firstName, lastName);
            var obligatoryCourses = await repository.GetCoursesAsync(_obligatoryCourseIds);

            obligatoryCourses.ForEach(c => employee.AttendedCourses.Add(c));
            employee.SuggestedBonus = CalculateSuggestedBonus(employee);

            return employee;
        }

        public ExternalEmployee CreateExternalEmployee(string firstName, string lastName, string company)
        {
            var employee = (ExternalEmployee)factory.CreateEmployee(
                firstName, lastName, company, true);

            return employee;
        }

        public async Task AddInternalEmployeeAsync(InternalEmployee internalEmployee)
        {
            repository.AddInternalEmployee(internalEmployee);
            await repository.SaveChangesAsync();
        }

        public void NotifyOfAbsence(Employee employee) =>
            OnEmployeeIsAbsent(new EmployeeIsAbsentEventArgs(employee.Id));

        protected virtual void OnEmployeeIsAbsent(
            EmployeeIsAbsentEventArgs eventArgs) => EmployeeIsAbsent?.Invoke(this, eventArgs);


        private int CalculateSuggestedBonus(InternalEmployee employee)
        {
            if (employee.YearsInService == 0)
            {
                return employee.AttendedCourses.Count * 100;
            }
            else
            {
                return employee.YearsInService
                    * employee.AttendedCourses.Count * 100;
            }
        }
    }
}
