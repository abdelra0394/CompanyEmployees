using AutoMapper;
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
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repositoryManager,ILoggerManager loggerManager,IMapper mapper)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
            _mapper = mapper;
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyInDB = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyInDB);
            _repository.Save();

            var copmanyDto = _mapper.Map<CompanyDto>(companyInDB);
            return copmanyDto;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
           
            var companies = _repository.Company
                .GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
            
        }

        public CompanyDto GetCompany(Guid companyId, bool trackChanges)
        {
            Company company = _repository.Company
                .GetCompany(companyId, trackChanges);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
    }
}
