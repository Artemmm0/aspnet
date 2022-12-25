using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public abstract class CompanyForManipulationDto
    {
        [Required(ErrorMessage = "Company name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Company address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Company country is required.")]
        public string Country { get; set; }

        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }
    }
}
