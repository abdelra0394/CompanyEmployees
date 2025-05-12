using System.ComponentModel.Design;
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

        public async Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployee(companyId, employeeEntity);
            await _repository.SaveAsync(); 

            return _mapper.Map<EmployeeDto>(employeeEntity);

        }

        public async Task DeleteEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);
            if (employee == null)
                throw new EmployeeNotFoundException(employeeId);

            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeesInDB = await _repository.Employee.GetAllEmployeesAsync(companyId, trackChanges);

            var employees = _mapper.Map<IEnumerable<EmployeeDto>>(employeesInDB);
            return employees;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeInDB = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
            if (employeeInDB == null)
                throw new EmployeeNotFoundException(id);

            var employee = _mapper.Map<EmployeeDto>(employeeInDB);
            return employee;
        }

        public async Task<(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId, bool companyTrackChanges, bool employeeTrackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, companyTrackChanges);
            if (company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeInDB = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, employeeTrackChanges);
            if (employeeInDB == null)
                throw new EmployeeNotFoundException(employeeId);

            var employeeForPatch = _mapper.Map<EmployeeForUpdateDto>(employeeInDB);

            return (employeeForPatch, employeeInDB);
        }

        public async Task SaveEmployeePatchAsync(EmployeeForUpdateDto employeeForPatch, Employee employeeEntity)
        {
            _mapper.Map(employeeForPatch, employeeEntity);
            await _repository.SaveAsync();
        }

        public async Task UpdateEmployeeAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employee, bool companyTrackChanges, bool employeeTrackChanges)
        {
            var companyEntity = await _repository.Company.GetCompanyAsync(companyId, companyTrackChanges);
            if (companyEntity == null)
                throw new CompanyNotFoundException(companyId);

            var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, employeeTrackChanges);
            if (employeeEntity == null)
                throw new EmployeeNotFoundException(employeeId);

            _mapper.Map(employee, employeeEntity);
            await _repository.SaveAsync();
        }
    }
}
