using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public abstract class ProductForManipulationDto
    {
        [Required(ErrorMessage = "Product name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }

        [Range(0.1, double.MaxValue, 
            ErrorMessage = "Product price is a required field and can't be lower than 0.1")]
        public double Price { get; set; }
    }
}
