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

        public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
        {
            var companyInDB = _mapper.Map<Company>(company);
            _repository.Company.CreateCompany(companyInDB);
            await _repository.SaveAsync();

            var copmanyDto = _mapper.Map<CompanyDto>(companyInDB);
            return copmanyDto;
        }

        public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies)
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
            await _repository.SaveAsync();

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntity);
            var ids = string.Join(",", companiesDto.Select(c => c.Id));

            return (companies: companiesDto, ids: ids);
        }

        public async Task DeleteCompany(Guid companyId,bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, false);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();

        }

        public Task DeleteCompanyAsync(Guid companyId, bool trachChanges)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
        {
           
            var companies = await _repository.Company
                .GetAllCompaniesAsync(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
            
        }

        public async Task<IEnumerable<CompanyDto>> GetCompaniesCollectionAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids == null || ids.Count() == 0)
            {
                throw new IdsOfCollectionBadRequest();
            }

            var companiesEntity = await _repository.Company.GetCompaniesCollectionAsync(ids, trackChanges);

            if (companiesEntity.Count() != ids.Count())
            {
                throw new CollectionBadRequest();
            }

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companiesEntity);

            return companiesDto;
        }

        public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
        {
            Company company = await _repository.Company
                .GetCompanyAsync(companyId, trackChanges);

            if (company == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            CompanyDto companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }

        public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto company, bool trackChanges)
        {
            var companyEntity = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
            if (companyEntity == null)
            {
                throw new CompanyNotFoundException(companyId);
            }

            _mapper.Map(company, companyEntity);
            await _repository.SaveAsync();

        }
    }
}
