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
        public IActionResult GetCompanies()
        {
            var companies = _serviceManager
                .CompanyService
                .GetAllCompanies(trackChanges: false);

            return Ok(companies);
            
        }

        [HttpGet("{id:guid}",Name ="CompanyById")]
        public IActionResult GetCompany(Guid id)
        {
            var company = _serviceManager
                .CompanyService
                .GetCompany(id, trackChanges: false);

            return Ok(company);
        }

        [HttpPost]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto company)
        {
            if(company == null)
            {
                return BadRequest("company object is null");
            }

            var companyDto = _serviceManager.CompanyService
                .CreateCompany(company);

            return CreatedAtRoute("CompanyById", new { id = companyDto.Id }, companyDto);
        }

        [HttpGet("collection/({ids})",Name ="CompaniesCollection")]
        public IActionResult CompaniesCollection([ModelBinder(binderType:typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = _serviceManager.CompanyService
                .GetCompaniesCollection(ids, trackChanges: false);

            return Ok(companies);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companies)
        {

            var companiesDto = _serviceManager.CompanyService
                .CreateCompanyCollection(companies);

            return CreatedAtRoute("CompaniesCollection", new { ids = companiesDto.ids }, companiesDto.companies);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteCompany(Guid id)
        {
            _serviceManager.CompanyService.DeleteCompany(id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            if(company == null)
            {
                return BadRequest("company dto object is null");
            }

            _serviceManager.CompanyService.UpdateCompany(id, company, true);
            return NoContent();
        }

    }
}
