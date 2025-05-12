using Entities.Models;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(Guid companyId, bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);

        Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

        Task DeleteEmployeeAsync(Guid companyId,Guid employeeId,bool trackChanges);

        Task UpdateEmployeeAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool companyTrackChanges, bool employeeTrackChanges);

        Task<(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId, bool companyTrackChanges, bool employeeTrackChanges);

        Task SaveEmployeePatchAsync(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity);
    }
}
