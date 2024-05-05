using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Exceptions.BadRequest;
using Entities.Exceptions.NotFound;
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

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies)
        {
            if(companies == null)
            {
                throw new CompanyCollectionBadRequest();
            }

            var companiesEntity = _mapper.Map<IEnumerable<Company>>(companies);
            foreach (var company in companiesEntity)
            {
                _repository.Company.CreateCompany(company);
            }
            _repository.Save();

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntity);
            var ids = string.Join(",", companiesDto.Select(c => c.Id));

            return (companies: companiesDto, ids: ids);
        }

        public void DeleteCompany(Guid companyId,bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, false);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            _repository.Company.DeleteCompany(company);
            _repository.Save();

        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
           
            var companies = _repository.Company
                .GetAllCompanies(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
            
        }

        public IEnumerable<CompanyDto> GetCompaniesCollection(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids == null || ids.Count() == 0)
            {
                throw new IdsOfCollectionBadRequest();
            }

            var companiesEntity = _repository.Company.GetCompaniesCollection(ids, trackChanges);

            if (companiesEntity.Count() != ids.Count())
            {
                throw new CollectionBadRequest();
            }

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntity);

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

        public void UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyId, trackChanges);
            if (companyEntity == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            _mapper.Map(company, companyEntity);
            _repository.Save();

        }
    }
}
