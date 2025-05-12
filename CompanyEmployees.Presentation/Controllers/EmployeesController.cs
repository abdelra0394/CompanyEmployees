using System.Threading.Tasks;
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
        public async Task<IActionResult> GetEmployees(Guid companyId)
        {
            var employees = await _serviceManager.EmployeeService
                .GetAllEmployeesAsync(companyId, trackChanges: false);
            return Ok(employees);
        }

        [HttpGet("{id:guid}",Name ="GetEmployee")]
        public async Task<IActionResult> GetEmployee(Guid companyId, Guid id)
        {
            var employee = await _serviceManager.EmployeeService
                .GetEmployeeAsync(companyId, id, trackChanges: false);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Guid companyId,EmployeeForCreationDto employee)
        {
            if(employee == null)
            {
                return BadRequest("EmployeeForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var employeeEntity = await _serviceManager.EmployeeService
                .CreateEmployeeAsync(companyId, employee,false);

            return CreatedAtRoute("GetEmployee", new { companyId, id = employeeEntity.Id }, employeeEntity);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid id)
        {
            await _serviceManager.EmployeeService.DeleteEmployeeAsync(companyId, id, false);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid companyId,Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if(employee == null)
            {
                return BadRequest("Employee dto object is null");
            }
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            await _serviceManager
                .EmployeeService
                .UpdateEmployeeAsync(companyId, id, employee,
                companyTrackChanges: false, employeeTrackChanges: true);

            return NoContent();

        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PatchEmployee(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("patchDoc is null");
            }
            var result = await _serviceManager.EmployeeService.GetEmployeeForPatchAsync(companyId, id, false, true);

            patchDoc.ApplyTo(result.employeeForPatch,ModelState);
            
            TryValidateModel(result.employeeForPatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }


            await _serviceManager.EmployeeService.SaveEmployeePatchAsync(result.employeeForPatch, result.employeeEntity);

            return NoContent();

        }

    }
}
