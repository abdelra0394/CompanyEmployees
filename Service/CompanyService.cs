﻿using Contracts;
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

        public CompanyService(IRepositoryManager repositoryManager,ILoggerManager loggerManager)
        {
            _repository = repositoryManager;
            _logger = loggerManager;
        }

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            try
            {
                var companies = _repository.Company
                    .GetAllCompanies(trackChanges);
                var companiesDto = companies.Select(c => new CompanyDto(
                    c.Id,
                    c.Name ?? "",
                    string.Join(' ',c.Address,c.Country)
                    ));
                return companiesDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex.Message}");
                throw;
            }
        }
    }
}
