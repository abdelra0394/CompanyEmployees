﻿using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);

        CompanyDto GetCompany(Guid companyId, bool trackChanges);

        CompanyDto CreateCompany(CompanyForCreationDto company);

        IEnumerable<CompanyDto> GetCompaniesCollection(IEnumerable<Guid> ids, bool trackChanges);

        (IEnumerable<CompanyDto>companies,string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companies);

        void DeleteCompany(Guid companyId,bool trachChanges);

        void UpdateCompany(Guid companyId, CompanyForUpdateDto company, bool trackChanges);
    }
}
