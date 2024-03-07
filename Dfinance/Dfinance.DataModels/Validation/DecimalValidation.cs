using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Validation
{
    public class DecimalValidation : ValidationAttribute
    {
        public int MaxDecPlaces { get; }
        public DecimalValidation(int maxDecPlace)
        {
            MaxDecPlaces=maxDecPlace;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value!=null)
            {
                decimal rate=(decimal)value;
                if (!decimal.TryParse(value.ToString(), out rate))
                {
                    return new ValidationResult(ErrorMessage);
                }
                int decPlace= BitConverter.GetBytes(decimal.GetBits(rate)[3])[2];
                if (decPlace>MaxDecPlaces)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }


        //protected override ValidationResult IsValid(object? value, ValidationContext context)
        //{
        //    var item = value;
        //  if(item != null)
        //    {
        //        string stringval=item.ToString();
        //        if(decimal.TryParse(stringval,out decimal decimalval)) {
        //            return ValidationResult.Success;
        //        }
        //        return new ValidationResult(ErrorMessage);
        //    }
        //    return ValidationResult.Success;

        //}
    }
}
