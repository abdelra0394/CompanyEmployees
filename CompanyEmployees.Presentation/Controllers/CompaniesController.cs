﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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

    }
}
