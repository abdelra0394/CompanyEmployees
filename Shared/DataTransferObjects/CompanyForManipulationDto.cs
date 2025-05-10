using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(30, ErrorMessage = "Max length for name is 30 characters")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Company country is required")]
        [MaxLength(30, ErrorMessage = "Max length for country is 30 characters")]
        public string Country { get; init; }

        [Required(ErrorMessage = "Company address is required")]
        [MaxLength(30, ErrorMessage = "Max length for address is 30 characters")]
        public string Address { get; init; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; init; }
    }
}
