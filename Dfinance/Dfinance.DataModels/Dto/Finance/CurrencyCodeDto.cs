using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Finance
{
    public class CurrencyCodeDto
    {
        [Required(ErrorMessage = "Code is mandatory!!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is mandatory!!")]
        public string Name { get; set; }
    }
}
