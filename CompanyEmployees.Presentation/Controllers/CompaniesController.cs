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

    }
}
