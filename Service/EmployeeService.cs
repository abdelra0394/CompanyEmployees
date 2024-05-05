using AutoMapper;
using Contracts;
using Entities.Exceptions.NotFound;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
        {
            _logger = loggerManager;
            _repository = repositoryManager;
            _mapper = mapper;
        }

        public EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployee(companyId, employeeEntity);
            _repository.Save();

            return _mapper.Map<EmployeeDto>(employeeEntity);

        }

        public void DeleteEmployee(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);
            if (employee == null)
                throw new EmployeeNotFoundException(employeeId);

            _repository.Employee.DeleteEmployee(employee);
            _repository.Save();
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeesInDB = _repository.Employee.GetAllEmployees(companyId, trackChanges);

            var employees = _mapper.Map<IEnumerable<EmployeeDto>>(employeesInDB);
            return employees;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeInDB = _repository.Employee.GetEmployee(companyId, id, trackChanges);
            if (employeeInDB == null)
                throw new EmployeeNotFoundException(id);

            var employee = _mapper.Map<EmployeeDto>(employeeInDB);
            return employee;
        }

        public void UpdateEmployee(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool companyTrackChanges, bool employeeTrackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyId, companyTrackChanges);
            if (companyEntity == null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = _repository.Employee.GetEmployee(companyId, employeeId, employeeTrackChanges);
            if (employeeEntity == null)
                throw new EmployeeNotFoundException(employeeId);

            _mapper.Map(employee, employeeEntity);
            _repository.Save();
        }
    }
}
