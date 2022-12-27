using EmployeeManagement.Entities;

namespace EmployeeManagement.Interfaces.Services
{
    public interface IPromotionService
    {
        Task<bool> PromoteInternalEmployeeAsync(InternalEmployee employee);
    }
}