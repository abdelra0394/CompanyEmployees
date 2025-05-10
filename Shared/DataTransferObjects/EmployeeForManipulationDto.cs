using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is required")]
        [MaxLength(30, ErrorMessage = "Max length for name is 30 characters")]
        public string Name { get; init; }

        [Range(18, int.MaxValue, ErrorMessage = "Employee age is required and can not be lower than 18")]
        public int Age { get; init; }

        [Required(ErrorMessage = "Employee position is required")]
        [MaxLength(30, ErrorMessage = "Max length for position is 30 characters")]
        public string Position { get; init; }
    };
}
