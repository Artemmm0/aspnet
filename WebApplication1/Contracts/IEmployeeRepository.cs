using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(Guid companyId, bool trackChanges);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(Guid companyId, bool trackChanges);
        Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
            EmployeeParameters employeeParameters, bool trackChanges);

        Employee GetEmployee(Guid companyId, Guid id, bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
        void CreateEmployeeForCompany(Guid companyId, Employee employee);

        IEnumerable<Employee> GetByIds(Guid companyId, IEnumerable<Guid> ids, bool trackChanges);
        Task<IEnumerable<Employee>> GetByIdsAsync(Guid companyId, IEnumerable<Guid> ids, bool trackChanges);

        void DeleteEmployee(Employee employee);
    }
}
