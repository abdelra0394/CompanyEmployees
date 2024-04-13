﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    internal sealed class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public EmployeeService(IRepositoryManager repositoryManager,ILoggerManager loggerManager,IMapper mapper)
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

        public IEnumerable<EmployeeDto> GetAllEmployees(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);

            var employeesInDB = _repository.Employee.GetAllEmployees(companyId, trackChanges);
            
            var employees = _mapper.Map<IEnumerable<EmployeeDto>>(employeesInDB);
            return employees;
        }

        public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if(company == null)
                throw new CompanyNotFoundException(companyId);

            var employeeInDB = _repository.Employee.GetEmployee(companyId, id, trackChanges);
            if(employeeInDB == null)
                throw new EmployeeNotFoundException(id);

            var employee = _mapper.Map<EmployeeDto>(employeeInDB);
            return employee;
        }
    }
}
