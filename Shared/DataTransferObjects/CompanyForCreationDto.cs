namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto(string Name, string Country, string Address,IEnumerable<EmployeeForCreationDto> Employees);
}
