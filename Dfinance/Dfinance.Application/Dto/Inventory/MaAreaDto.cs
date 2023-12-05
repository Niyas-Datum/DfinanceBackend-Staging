using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Dto.Inventory
{
    public class MaAreaDto
    {
        [Required(ErrorMessage ="Code is Mandatory")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Mandatory")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Group { get; set; }
        public bool IsGroup { get; set; }     
        public bool? Active { get; set; }
       
    }
}
