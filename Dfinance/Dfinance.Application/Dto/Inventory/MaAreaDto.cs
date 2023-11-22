using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Dto.Inventory
{
    public class MaAreaDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Group { get; set; }
        public bool IsGroup { get; set; }     
        public bool? Active { get; set; }
       
    }
}
