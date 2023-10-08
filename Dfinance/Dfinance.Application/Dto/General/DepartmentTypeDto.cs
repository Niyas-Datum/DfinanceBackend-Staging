using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Dto
{
    public class DepartmentTypeDto
    {
        public String Department {  get; set; }
        public int CreatedBy {  get; set; }
        public DateTime CreatedOn {  get; set; }
        public int CreatedBranchID {  get; set; }
    }
}
