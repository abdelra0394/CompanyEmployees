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
        IEnumerable<EmployeeDto> GetAllEmployees(Guid companyId, bool trackChanges);
        EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);

        EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);

        void DeleteEmployee(Guid companyId,Guid employeeId,bool trackChanges);

        void UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool companyTrackChanges, bool employeeTrackChanges);

        (EmployeeForUpdateDto employeeForPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid employeeId, bool companyTrackChanges, bool employeeTrackChanges);

        void SaveEmployeePatch(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity);
    }
}
