using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public EmployeesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public IActionResult GetEmployees(Guid companyId)
        {
            var employees = _serviceManager.EmployeeService
                .GetAllEmployees(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}",Name ="GetEmployee")]
        public IActionResult GetEmployee(Guid companyId, Guid id)
        {
            var employee = _serviceManager.EmployeeService
                .GetEmployee(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee(Guid companyId,EmployeeForCreationDto employee)
        {
            if(employee == null)
            {
                return BadRequest("EmployeeForCreationDto object is null");
            }

            var employeeEntity = _serviceManager.EmployeeService
                .CreateEmployee(companyId, employee,false);

            return CreatedAtRoute("GetEmployee", new { companyId, id = employeeEntity.Id }, employeeEntity);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult DeleteEmployee(Guid companyId, Guid id)
        {
            _serviceManager.EmployeeService.DeleteEmployee(companyId, id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public IActionResult UpdateEmployee(Guid companyId,Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if(employee == null)
            {
                return BadRequest("Employee dto object is null");
            }
            _serviceManager
                .EmployeeService
                .UpdateEmployee(companyId, id, employee,
                companyTrackChanges: false, employeeTrackChanges: true);

            return NoContent();

        }

        [HttpPatch("{id:guid}")]
        public IActionResult PatchEmployee(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("patchDoc is null");
            }
            var result =  _serviceManager.EmployeeService.GetEmployeeForPatch(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeForPatch);

            _serviceManager.EmployeeService.SaveEmployeePatch(result.employeeForPatch, result.employeeEntity);

            return NoContent();

        }

    }
}
