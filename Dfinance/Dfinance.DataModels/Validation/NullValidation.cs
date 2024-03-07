using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Validation
{
    public class NullValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext context)
        {            
            var item = value;
            if(item == null) 
                return new ValidationResult(ErrorMessage);
            return ValidationResult.Success;           
           
        }
        
    }
}

