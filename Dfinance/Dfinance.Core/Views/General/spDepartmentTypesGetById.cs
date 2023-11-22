using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views
{
    public class spDepartmentTypesGetById
    {
        public int ID {  get; set; }
        public string Department { get; set; }
        public int CreatedBranchID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte ActiveFlag { get; set; }
    }
}
