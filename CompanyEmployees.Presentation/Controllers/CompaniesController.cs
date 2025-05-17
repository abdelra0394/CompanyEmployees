using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CompaniesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _serviceManager
                .CompanyService
                .GetAllCompaniesAsync(trackChanges: false);

            return Ok(companies);
            
        }

        [HttpGet("{id:guid}",Name ="CompanyById")]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _serviceManager
                .CompanyService
                .GetCompanyAsync(id, trackChanges: false);

            return Ok(company);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var companyDto = await _serviceManager.CompanyService
                .CreateCompanyAsync(company);

            return CreatedAtRoute("CompanyById", new { id = companyDto.Id }, companyDto);
        }

        [HttpGet("collection/({ids})",Name ="CompaniesCollection")]
        public async Task<IActionResult> CompaniesCollection([ModelBinder(binderType:typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _serviceManager.CompanyService
                .GetCompaniesCollectionAsync(ids, trackChanges: false);

            return Ok(companies);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {

            var companiesDto = await _serviceManager.CompanyService
                .CreateCompanyCollectionAsync(companies);

            return CreatedAtRoute("CompaniesCollection", new { ids = companiesDto.ids }, companiesDto.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _serviceManager.CompanyService.DeleteCompanyAsync(id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _serviceManager.CompanyService.UpdateCompanyAsync(id, company, true);
            return NoContent();
        }

    }
}
