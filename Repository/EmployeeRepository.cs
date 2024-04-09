using Contracts;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            
        }

        public IEnumerable<Employee> GetAllEmployees(Guid companyId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId == companyId,trackChanges)
                .OrderBy(e => e.Name)
                .ToList();
        }

        public Employee GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            return FindByCondition(e => e.Id == id && e.CompanyId == companyId,trackChanges)
                .SingleOrDefault();
        }
    }
}
