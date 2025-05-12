using Shared.DataTransferObjects;
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
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);

        Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);

        Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);

        Task<IEnumerable<CompanyDto>> GetCompaniesCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);

        Task<(IEnumerable<CompanyDto>companies,string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companies);

        Task DeleteCompanyAsync(Guid companyId,bool trachChanges);

        Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto company, bool trackChanges);
    }
}
