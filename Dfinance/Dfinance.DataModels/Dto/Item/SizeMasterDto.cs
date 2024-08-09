using Dfinance.DataModels.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.DataModels.Dto.Item
{
    public class SizeMasterDto
    {
        public int? Id { get; set; }
        [NullValidation(ErrorMessage = "Code is mandatory!!")]
        public string Code { get; set; }
        [NullValidation(ErrorMessage = "Name is mandatory!!")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public SizeMasterDto()
        {
            Active = false;
        }
    }

}
