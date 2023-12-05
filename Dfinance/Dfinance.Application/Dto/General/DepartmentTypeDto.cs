using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Dto
{
    public class DepartmentTypeDto
    {
        [Required(ErrorMessage = "Department is mandatory!!")]
        public String Department {  get; set; }
      
       
    }
}
